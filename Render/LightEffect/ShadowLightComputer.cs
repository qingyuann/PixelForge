using System.Numerics;
using Component;
using Entitas;
using Light;
using PixelForge;
using PixelForge.Tools;
using pp;
using Silk.NET.OpenGL;

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
	Texture _lightMap;
	RenderTexture _shadowMap;
	byte[] _screenData = Array.Empty<byte>();
	byte[] _lightData = Array.Empty<byte>();
	RenderFullscreen _shadowLightShadowMap;
	RenderFullscreen _shadowLightDraw;
	RenderFullscreen _mergeTwoTex_add;
	GaussianBlurComputer _gaussianBlurComputer;

	public ShadowLightComputer() {
		_shadowMap = TexturePool.GetRT( ShadowLightPrecisionAngular, 1, false );
		_shadowLightShadowMap = new RenderFullscreen( "Blit_CustomUVScale.vert", "ShadowLightShadowMap.frag" );
		_shadowLightDraw = new RenderFullscreen( "Blit.vert", "ShadowLightDraw.frag" );
		_mergeTwoTex_add = new RenderFullscreen( "Blit.vert", "MergeTwoTex_add.frag" );
		_gaussianBlurComputer = new GaussianBlurComputer();
		//para: high quality:2,5, low quality: 5,3
		_gaussianBlurComputer.SetParams( new GaussianBlurComponent(){
			Offset = 4f,
			Iterations = 3
		} );

	}

	public override void Render( RenderTexture rt ) {
		//////////////////////////////////////////////////////
		//// step1: cut the light map from render texture ////
		//////////////////////////////////////////////////////
		//get the screen data
		if( _screenData.Length != rt.Width * rt.Height * 4 ) {
			_screenData = new byte[rt.Width * rt.Height * 4];
		}

		rt.GetImage( _screenData );

		//get the light data
		int radiusPixelSize = (int)Transform.WorldToPixelSize( _radius );
		if( _lightData.Length != radiusPixelSize * 2 * radiusPixelSize * 2 * 4 ) {
			_lightData = new byte[radiusPixelSize * 2 * radiusPixelSize * 2 * 4];
		}

		//clear the light data
		Array.Fill( _lightData, (byte)0 );

		Vector2 posPixCenter = Transform.WorldToPixel( _position, true );

		//copy the screen data to light data within the lightMap
		//concurrent version!!! Best performance!!!
		Parallel.For( 0, radiusPixelSize * 2, j => {

			var screenY = (int)posPixCenter.Y - radiusPixelSize + j;
			if( screenY < 0 || screenY >= rt.Height ) {
				return;
			}

			var lightY = j;
			var screenX = (int)posPixCenter.X - radiusPixelSize;
			var length = radiusPixelSize * 2;
			var lightX = 0;
			// if screen left < 0, cut the light left
			if( screenX < 0 ) {
				lightX = -screenX;
				length += screenX;
				screenX = 0;
			}

			// if screen right > screen right, cut the light right
			if( screenX + length > rt.Width ) {
				length -= ( screenX + length - rt.Width );
			}

			if( length <= 0 ) {
				return;
			}

			var screenIndex = Image.TryGetIndex( screenX, screenY, rt.Width, rt.Height );
			if( screenIndex is null ) {
				Debug.LogError( "screenIndex is null" );
				return;
			}

			var lightIndex = Image.TryGetIndex( lightX, lightY, radiusPixelSize * 2, radiusPixelSize * 2 );
			if( lightIndex is null ) {
				Debug.LogError( "lightIndex is null" );
				return;
			}

			Array.Copy( _screenData, screenIndex.Value * 4, _lightData, lightIndex.Value * 4, ( length - 1 ) * 4 );
		} );
		/////////////////////////////////////////////////////
		//// step2: render the shadow map from light map ////
		/////////////////////////////////////////////////////
		_lightMap = TexturePool.GetTex( (uint)radiusPixelSize * 2, (uint)radiusPixelSize * 2 );
		_lightMap.UpdateImageContent( _lightData, (uint)radiusPixelSize * 2, (uint)radiusPixelSize * 2 );
		var uvScale = GameSetting.WindowWidth / (float)ShadowLightPrecisionAngular;
		_shadowMap.RenderToRt();
		_shadowLightShadowMap.SetTexture( "_BlitTexture", _lightMap );
		_shadowLightShadowMap.SetUniform( "resolution", Math.Min( ShadowLightPrecisionMarch * _radius, ShadowLightPrecisionMarch ) );
		_shadowLightShadowMap.SetUniform( "_UVScale", uvScale );
		_shadowLightShadowMap.Draw();
		GlobalVariable.GL.Finish();
		TexturePool.ReturnTex( _lightMap );

		// #region testShadowMap
		//  _shadowMap.GetImage(_shadowData);
		//  var col = new List<Vector4>();
		//  for (int i = 0; i < ShadowLightPrecisionAngular; i++)
		//  {
		//  	col.Add(Image.TryGetColorPixelRGBA( _shadowData, i, 0, ShadowLightPrecisionAngular, 1 ));
		//  }
		//  col.Select((i,j)=>new {i,j}).ToList().ForEach(i=>Console.WriteLine(i.j + " " + i.i));
		// #endregion

		////////////////////////////////////
		//// step3: render the 2d light ////
		////////////////////////////////////
		RenderTexture tempRt = TexturePool.GetRT( (uint)rt.Width, (uint)rt.Height, false );
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

		//////////////////////////////
		//// step4: blur the light////
		//////////////////////////////
		_gaussianBlurComputer.Render( tempRt );

		//////////////////////////////////////
		//// step5: merge the light to rt ////
		//////////////////////////////////////
		var tempRt2 = TexturePool.GetRT( (uint)rt.Width, (uint)rt.Height, false );
		tempRt2.RenderToRt();
		_mergeTwoTex_add.SetTexture( "_MergeTexture", tempRt );
		Blitter.Blit( rt, tempRt2, _mergeTwoTex_add );
		Blitter.Blit( tempRt2, rt );
		// Blitter.Blit( tempRt, rt );

		TexturePool.ReturnRT( tempRt );
		TexturePool.ReturnRT( tempRt2 );
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