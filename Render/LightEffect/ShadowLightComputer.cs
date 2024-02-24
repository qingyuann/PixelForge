using System.Numerics;
using Component;
using Entitas;
using Light;
using PixelForge;
using PixelForge.Tools;
using pp;
using Silk.NET.OpenGL;
using System.Buffers;
using System.Diagnostics;

namespace Render.PostEffect;

public class ShadowLightComputer : LightEffectComputer {
	//sooooo expensive...
	const int ShadowLightPrecisionAngular = 360;
	const int ShadowLightPrecisionMarch = 200;
	Vector3 _color;
	float _intensity;
	Vector2 _position;
	float _radius;
	float _volume;
	float _radialFallOff;
	float _edgeInfringe;
	RenderTexture _shadowMap;
	RenderFullscreen _shadowLightLightMap;
	RenderFullscreen _shadowLightShadowMap;
	RenderFullscreen _shadowLightDraw;
	RenderFullscreen _mergeTwoTex_add;
	GaussianBlurComputer _gaussianBlurComputer;

	public ShadowLightComputer() {
		_shadowMap = TexturePool.GetRT( ShadowLightPrecisionAngular, 1, false );
		_shadowLightLightMap = new RenderFullscreen( "Blit.vert", "ShadowLightLightMap.frag" );
		_shadowLightShadowMap = new RenderFullscreen( "Blit_CustomUVScale.vert", "ShadowLightShadowMap.frag" );
		_shadowLightDraw = new RenderFullscreen( "Blit.vert", "ShadowLightDraw.frag" );
		_mergeTwoTex_add = new RenderFullscreen( "Blit.vert", "MergeTwoTex_add.frag" );
		_gaussianBlurComputer = new GaussianBlurComputer();
		//para: high quality:2,5, low quality: 5,3
		_gaussianBlurComputer.SetParams( new GaussianBlurComponent(){
			Offset = 4f,
			Iterations = 2
		} );

	}

	public override void Render( RenderTexture rt ) {
		////////////////////////////////////////
		//// step0: prepare the light data ////
		///////////////////////////////////////
		int radiusPixelSize = (int)Transform.WorldToPixelSize( _radius );
		Vector2 posPixCenter = Transform.WorldToPixel( _position, true );
		Vector2 posLightMapScreenCenter = new Vector2( radiusPixelSize, radiusPixelSize );

		RenderTexture tempRt2 = TexturePool.GetRT( (uint)rt.Width, (uint)rt.Height, false );
		RenderTexture tempRt = TexturePool.GetRT( (uint)rt.Width, (uint)rt.Height, false );
		RenderTexture lightMap = TexturePool.GetRT( (uint)radiusPixelSize * 2, (uint)radiusPixelSize * 2 );


		//////////////////////////////////////////////////////
		//// step1: cut the light map from render texture ////
		//////////////////////////////////////////////////////
		float lightMapUvMoveX = ( posPixCenter.X - posLightMapScreenCenter.X ) / GameSetting.WindowWidth; //from light center to screen center
		float lightMapUvMoveY = ( posPixCenter.Y - posLightMapScreenCenter.Y ) / GameSetting.WindowHeight; //from light center to screen center
		Vector2 lightMapUvMove = new Vector2( lightMapUvMoveX, lightMapUvMoveY ); //from light center to screen center
		_shadowLightLightMap.SetUniform( "lightMapUVMove", lightMapUvMove );
		Blitter.Blit( rt, lightMap, _shadowLightLightMap );


		/////////////////////////////////////////////////////
		//// step2: render the shadow map from light map ////
		/////////////////////////////////////////////////////
		var uvScale = GameSetting.WindowWidth / (float)ShadowLightPrecisionAngular;
		_shadowMap.RenderToRt();
		_shadowLightShadowMap.SetTexture( "_BlitTexture", lightMap );
		_shadowLightShadowMap.SetUniform( "resolution", Math.Min( ShadowLightPrecisionMarch * _radius, ShadowLightPrecisionMarch ) );
		_shadowLightShadowMap.SetUniform( "_UVScale", uvScale );
		_shadowLightShadowMap.Draw();
		TexturePool.ReturnTex( lightMap );

		////////////////////////////////////
		//// step3: render the 2d light ////
		////////////////////////////////////
		tempRt.RenderToRt();
		_shadowLightDraw.SetTexture( "_ShadowMap", _shadowMap );
		_shadowLightDraw.SetUniform( "screenW", rt.Width );
		_shadowLightDraw.SetUniform( "screenH", rt.Height );
		_shadowLightDraw.SetUniform( "lightPosPix", posPixCenter );
		_shadowLightDraw.SetUniform( "lightRadiusPix", radiusPixelSize );
		_shadowLightDraw.SetUniform( "lightColor", _color );
		_shadowLightDraw.SetUniform( "falloff", _radialFallOff );
		_shadowLightDraw.SetUniform( "intensity", _intensity );
		_shadowLightDraw.SetUniform( "volumeIntensity", _volume );
		_shadowLightDraw.SetUniform( "edgeInfringe", _edgeInfringe );
		Blitter.Blit( null, tempRt, _shadowLightDraw );
		GlobalVariable.GL.Finish();

		//////////////////////////////
		//// step4: blur the light////
		//////////////////////////////
		_gaussianBlurComputer.Render( tempRt );

		//////////////////////////////////////
		//// step5: merge the light to rt ////
		//////////////////////////////////////
		tempRt2.RenderToRt();
		_mergeTwoTex_add.SetTexture( "_MergeTexture", tempRt );
		Blitter.Blit( rt, tempRt2, _mergeTwoTex_add );
		Blitter.Blit( tempRt2, rt );

		/////////////////////////////////
		//// step6: release the data ////
		/////////////////////////////////
		// TexturePool.ReturnRT( tempRt );
		TexturePool.ReturnRT( tempRt );
		TexturePool.ReturnRT( tempRt2 );
		TexturePool.ReturnRT( lightMap );


	}

	public override void SetParams( IComponent param ) {
		if( param is ShadowLightComponent globalLightComponent ) {
			_color = globalLightComponent.Color;
			_intensity = globalLightComponent.Intensity;
			_radius = globalLightComponent.Radius;
			_volume = globalLightComponent.Volume;
			_radialFallOff = globalLightComponent.RadialFallOff;
			_edgeInfringe = globalLightComponent.EdgeInfringe;
		}

		if( param is PositionComponent positionComponent ) {
			_position = new Vector2( positionComponent.X, positionComponent.Y );
		}
	}

	public override void Dispose() {
	}
}