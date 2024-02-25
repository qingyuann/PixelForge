﻿using System.Numerics;
using Component;
using Entitas;
using Light;
using PixelForge;
using PixelForge.Tools;
using pp;
using Silk.NET.OpenGL;
using System.Buffers;
using System.Diagnostics;
using Debug = PixelForge.Debug;

namespace Render.PostEffect;

public class ShadowLightComputer : LightEffectComputer {
	const int MaxResolution = 500;

	//sooooo expensive...
	const int AngularPrecision = 360 * 2;
	const int RadiusPrecision = 500;
	List<Vector3> _color = new List<Vector3>();
	List<float> _intensity = new List<float>();
	List<Vector2> _position = new List<Vector2>();
	List<float> _radius = new List<float>();
	List<float> _volume = new List<float>();
	List<float> _radialFallOff = new List<float>();
	List<float> _edgeInfringe = new List<float>();
	RenderFullscreen _shadowLightLightMap;
	RenderFullscreen _shadowLightShadowMap;
	RenderFullscreen _shadowLightDraw;
	RenderFullscreen _mergeLight;
	RenderFullscreen _mergeTwoTex_add;
	GaussianBlurComputer _gaussianBlurComputer;
	List<RenderTexture> lightTextures = new List<RenderTexture>();
	public ShadowLightComputer() {
		_shadowLightLightMap = new RenderFullscreen( "Blit_CustomUVScale.vert", "ShadowLightLightMap.frag" );
		_shadowLightShadowMap = new RenderFullscreen( "Blit_CustomUVScale.vert", "ShadowLightShadowMap.frag" );
		_shadowLightDraw = new RenderFullscreen( "Blit_CustomUVScale.vert", "ShadowLightDraw.frag" );
		_mergeTwoTex_add = new RenderFullscreen( "Blit.vert", "MergeTwoTex_add.frag" );
		_mergeLight = new RenderFullscreen( "Blit.vert", "QuadBasic.frag" );
		_gaussianBlurComputer = new GaussianBlurComputer();
		_gaussianBlurComputer.SetParams( new GaussianBlurComponent(){
			Offset = 2f,
			Iterations = 1
		} );

	}

	public override void Render( RenderTexture rt ) {
		// a texture to store the merged light
		RenderTexture mergeLightRT = TexturePool.GetRT( (uint)( rt.Width ), (uint)( rt.Height ), false );

		for( int i = 0; i < _radius.Count; i++ ) {
			/////////////////////////////////////////
			//// step -1: down sample the screen ////
			/////////////////////////////////////////
			int rtDownRatio = 1;
			while( MathF.Max( rt.Width / rtDownRatio, rt.Height / rtDownRatio ) > MaxResolution ) {
				rtDownRatio *= 2;
			}
			////////////////////////////////////////
			//// step0: prepare the light data ////
			///////////////////////////////////////
			// position of light in pixel
			Vector2 posPixCenter = Transform.WorldToPixel( _position[i], true );

			RenderTexture shadowMap = TexturePool.GetRT( AngularPrecision, 1, false );
			RenderTexture lightRt = TexturePool.GetRT( (uint)( rt.Width / rtDownRatio ), (uint)( rt.Height / rtDownRatio ), false );

			// if radius is too large, cut it to the screen size
			int radiusPixel = Transform.WorldToPixelSize( _radius[i] );
			int halfWidth = rt.Width / 2;
			int halfHeight = rt.Height / 2;
			int radiusPixelSizeXCut = (int)MathF.Min( radiusPixel, halfWidth );
			int radiusPixelSizeYCut = (int)MathF.Min( radiusPixel, halfHeight );
			RenderTexture lightMap = TexturePool.GetRT( (uint)( radiusPixelSizeXCut * 2 / rtDownRatio ), (uint)( radiusPixelSizeYCut * 2 / rtDownRatio ) );
			_radius[i] = Transform.PixelToWorldSize( MathF.Max( radiusPixelSizeXCut, radiusPixelSizeYCut ) );

			//////////////////////////////////////////////////////
			//// step1: cut the light map from render texture ////
			//////////////////////////////////////////////////////
			float lightMapUvMoveX = ( posPixCenter.X - radiusPixelSizeXCut ) / GameSetting.WindowWidth ; //from light center to screen center
			float lightMapUvMoveY = ( posPixCenter.Y - radiusPixelSizeYCut ) / GameSetting.WindowHeight ; //from light center to screen center
			Vector2 lightMapUvMove = new Vector2( lightMapUvMoveX, lightMapUvMoveY ); //from light center to screen center
			_shadowLightLightMap.SetUniform( "lightMapUVMove", lightMapUvMove );
			_shadowLightLightMap.SetUniform( "_UVScale", rtDownRatio );
			Blitter.Blit( rt, lightMap, _shadowLightLightMap );

			/////////////////////////////////////////////////////
			//// step2: render the shadow map from light map ////
			/////////////////////////////////////////////////////
			var uvScale = GameSetting.WindowWidth / (float)AngularPrecision;
			_shadowLightShadowMap.SetTexture( "_BlitTexture", lightMap );
			_shadowLightShadowMap.SetUniform( "lightRadius", RadiusPrecision * _radius[i] );
			_shadowLightShadowMap.SetUniform( "_UVScale", uvScale );
			Blitter.Blit( null, shadowMap, _shadowLightShadowMap );

			////////////////////////////////////
			//// step3: render the 2d light ////
			////////////////////////////////////
			lightRt.RenderToRt();
			_shadowLightDraw.SetTexture( "_ShadowMap", shadowMap );
			_shadowLightDraw.SetUniform( "screenW", rt.Width );
			_shadowLightDraw.SetUniform( "screenH", rt.Height );
			_shadowLightDraw.SetUniform( "lightPosPix", posPixCenter );
			_shadowLightDraw.SetUniform( "lightRadiusPix", MathF.Max( radiusPixelSizeXCut, radiusPixelSizeYCut ) );
			_shadowLightDraw.SetUniform( "lightColor", _color[i] );
			_shadowLightDraw.SetUniform( "falloff", _radialFallOff[i] );
			_shadowLightDraw.SetUniform( "intensity", _intensity[i] );
			_shadowLightDraw.SetUniform( "volumeIntensity", _volume[i] );
			_shadowLightDraw.SetUniform( "edgeInfringe", _edgeInfringe[i] );
			_shadowLightDraw.SetUniform( "_UVScale", rtDownRatio );
			Blitter.Blit( null, lightRt, _shadowLightDraw );
			GlobalVariable.GL.Finish();

			lightTextures.Add( lightRt );
			TexturePool.ReturnRT( lightMap );
			TexturePool.ReturnRT( shadowMap );
		}

		///////////////////////////////
		//// step4: merge all light////
		///////////////////////////////
		mergeLightRT.RenderToRt();
		GlobalVariable.GL.Clear( (uint)GLEnum.ColorBufferBit | (uint)GLEnum.DepthBufferBit );
		GlobalVariable.GL.Enable( EnableCap.Blend );
		GlobalVariable.GL.BlendFunc( BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha );
		foreach( RenderTexture lightRt in lightTextures ) {
			_mergeLight.SetTexture( "MainTex", lightRt );
			_mergeLight.Draw();
		}
		GlobalVariable.GL.Disable( EnableCap.Blend );

		///////////////////////////////
		//// step5: blur the light ////
		///////////////////////////////
		_gaussianBlurComputer.Render( mergeLightRT );

		//////////////////////////////////////
		//// step6: merge the light to rt ////
		//////////////////////////////////////
		rt.RenderToRt();
		_mergeLight.SetTexture( "MainTex", mergeLightRT );
		_mergeLight.Draw();

		/////////////////////////////////
		//// step7: release the data ////
		/////////////////////////////////
		lightTextures.ForEach( TexturePool.ReturnRT );
		lightTextures.Clear();
		TexturePool.ReturnRT( mergeLightRT );
	}

	public override void SetParams( List<(ILightComponent, PositionComponent)> param ) {
		ClearPara();
		for( int i = 0; i < param.Count; i++ ) {
			if( param[i].Item1 is ShadowLightComponent globalLightComponent ) {
				if( globalLightComponent.Radius <= 0 || globalLightComponent.Intensity <= 0 || globalLightComponent.Volume <= 0 || globalLightComponent.RadialFallOff <= 0 || globalLightComponent.EdgeInfringe <= 0 || globalLightComponent.Color == Vector3.Zero || globalLightComponent.Layers == null || globalLightComponent.Layers.Length == 0 ) {
					Debug.Log( "Light Component has invalid value" );
					return;
				}
				_color.Add( globalLightComponent.Color );
				_intensity.Add( globalLightComponent.Intensity );
				_radius.Add( globalLightComponent.Radius );
				_volume.Add( globalLightComponent.Volume );
				_radialFallOff.Add( globalLightComponent.RadialFallOff );
				_edgeInfringe.Add( globalLightComponent.EdgeInfringe );
			}

			var positionComponent = param[i].Item2;
			_position.Add( new Vector2( positionComponent.X, positionComponent.Y ) );
		}
	}


	void ClearPara() {
		_color.Clear();
		_intensity.Clear();
		_position.Clear();
		_radius.Clear();
		_volume.Clear();
		_radialFallOff.Clear();
		_edgeInfringe.Clear();
	}

	public override void Dispose() {
	}
}