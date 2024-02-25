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
	const int AngularPrecision = 360 * 2;
	const int RadiusPrecision = 500;
	Vector3 _color;
	float _intensity;
	Vector2 _position;
	float _radius;
	float _volume;
	float _radialFallOff;
	float _edgeInfringe;
	RenderFullscreen _shadowLightLightMap;
	RenderFullscreen _shadowLightShadowMap;
	RenderFullscreen _shadowLightDraw;
	RenderFullscreen _mergeTwoTex_add;
	GaussianBlurComputer _gaussianBlurComputer;

	public ShadowLightComputer() {
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
		// position of light in pixel
		Vector2 posPixCenter = Transform.WorldToPixel( _position, true );

		RenderTexture shadowMap = TexturePool.GetRT( AngularPrecision, 1, false );
		RenderTexture tempRt2 = TexturePool.GetRT( (uint)rt.Width, (uint)rt.Height, false );
		RenderTexture tempRt = TexturePool.GetRT( (uint)rt.Width, (uint)rt.Height, false );

		// if radius is too large, cut it to the screen size
		int radiusPixel = Transform.WorldToPixelSize( _radius );
		const int halfWidth = GameSetting.WindowWidth / 2;
		const int halfHeight = GameSetting.WindowHeight / 2;
		int radiusPixelSizeXCut =(int)MathF.Min( radiusPixel,halfWidth);
		int radiusPixelSizeYCut = (int)MathF.Min( radiusPixel,halfHeight);
		RenderTexture lightMap = TexturePool.GetRT( (uint)radiusPixelSizeXCut * 2, (uint)radiusPixelSizeYCut * 2 );
		_radius = Transform.PixelToWorldSize( MathF.Max( radiusPixelSizeXCut, radiusPixelSizeYCut ) );
		
		//////////////////////////////////////////////////////
		//// step1: cut the light map from render texture ////
		//////////////////////////////////////////////////////
		float lightMapUvMoveX = ( posPixCenter.X - radiusPixelSizeXCut ) / GameSetting.WindowWidth; //from light center to screen center
		float lightMapUvMoveY = ( posPixCenter.Y - radiusPixelSizeYCut ) / GameSetting.WindowHeight; //from light center to screen center
		Vector2 lightMapUvMove = new Vector2( lightMapUvMoveX, lightMapUvMoveY ); //from light center to screen center
		_shadowLightLightMap.SetUniform( "lightMapUVMove", lightMapUvMove );
		Blitter.Blit( rt, lightMap, _shadowLightLightMap );

		/////////////////////////////////////////////////////
		//// step2: render the shadow map from light map ////
		/////////////////////////////////////////////////////
		var uvScale = GameSetting.WindowWidth / (float)AngularPrecision;
		_shadowLightShadowMap.SetTexture( "_BlitTexture", lightMap );
		_shadowLightShadowMap.SetUniform( "lightRadius", RadiusPrecision * _radius );
		_shadowLightShadowMap.SetUniform( "_UVScale", uvScale );
		Blitter.Blit( null, shadowMap, _shadowLightShadowMap );

		////////////////////////////////////
		//// step3: render the 2d light ////
		////////////////////////////////////
		tempRt.RenderToRt();
		_shadowLightDraw.SetTexture( "_ShadowMap", shadowMap );
		_shadowLightDraw.SetUniform( "screenW", rt.Width );
		_shadowLightDraw.SetUniform( "screenH", rt.Height );
		_shadowLightDraw.SetUniform( "lightPosPix", posPixCenter );
		_shadowLightDraw.SetUniform( "lightRadiusPix", MathF.Max( radiusPixelSizeXCut, radiusPixelSizeYCut ) );
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
		// _gaussianBlurComputer.Render( tempRt );

		//////////////////////////////////////
		//// step5: merge the light to rt ////
		//////////////////////////////////////
		tempRt2.RenderToRt();
		_mergeTwoTex_add.SetTexture( "_MergeTexture", tempRt );
		Blitter.Blit( rt, tempRt2, _mergeTwoTex_add );
		// Blitter.Blit( tempRt2, rt );
		Blitter.Blit( tempRt2, rt );

		/////////////////////////////////
		//// step6: release the data ////
		/////////////////////////////////
		TexturePool.ReturnRT( tempRt );
		TexturePool.ReturnRT( tempRt2 );
		TexturePool.ReturnRT( lightMap );
		TexturePool.ReturnRT( shadowMap );
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