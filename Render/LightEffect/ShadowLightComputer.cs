using System.Numerics;
using Entitas;
using Light;
using PixelForge;
using PixelForge.Tools;
using pp;
using Silk.NET.OpenGL;

namespace Render.PostEffect;

public class ShadowLightComputer : LightEffectComputer {
	Vector3 _color;
	float _intensity;
	Vector2 _position;
	float _radius;
	float _volume;
	float _radialFallOff;
	float _angle;
	Texture _lightMap;
	RenderTexture _shadowMap;
	byte[] _screenData = Array.Empty<byte>();
	byte[] _lightData = Array.Empty<byte>();
	byte[] _shadowData = Array.Empty<byte>();
	RenderFullscreen _shadowRenderFullscreen;
	public ShadowLightComputer() {
		_shadowMap = TexturePool.GetRT( 360, 1, false );
		_shadowRenderFullscreen = new RenderFullscreen( "Blit.vert", "ShadowLightShadowMap.frag" );
		_shadowData = new byte[360 * 4];
	}

	public override void Render( RenderTexture rt ) {
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
		for( int i = 0; i < radiusPixelSize * 2; i++ ) {
			var screenY = (int)posPixCenter.Y - radiusPixelSize + i;
			if( screenY < 0 || screenY >= rt.Height ) {
				continue;
			}
			var lightY = i;
			var screenX = (int)posPixCenter.X - radiusPixelSize;
			var length = radiusPixelSize * 2;
			var lightX = 0;
			if( screenX < 0 ) {
				lightX = -screenX;
				length += screenX;
				screenX = 0;
				// Console.WriteLine( "light left < 0, lightX=" + lightX + ", length=" + length );
			}
			if( screenX + length > rt.Width ) {
				length = rt.Width - screenX;
				// Console.WriteLine( "light right > screen right, length=" + length );
			}
			if( length <= 0 ) {
				// Console.WriteLine( "length < 0" );
				continue;
			}
			var screenIndex = Image.TryGetIndex( screenX, screenY, rt.Width, rt.Height );
			if( screenIndex is null ) {
				Console.WriteLine( "screenIndex is null" );
				continue;
			}
			var lightIndex = Image.TryGetIndex( lightX, lightY, radiusPixelSize * 2, radiusPixelSize * 2 );
			if( lightIndex is null ) {
				Console.WriteLine( "lightIndex is null" );
				continue;
			}

			Array.Copy( _screenData, screenIndex.Value * 4, _lightData, lightIndex.Value * 4, length * 4 );
			// var color = Image.TryGetColorPixelRGBA( _lightData, radiusPixelSize*2-1, 0, radiusPixelSize * 2, radiusPixelSize * 2 );
			// Console.WriteLine( color );
		}
		_lightMap = TexturePool.GetTex( (uint)radiusPixelSize * 2, (uint)radiusPixelSize * 2 );
		_lightMap.UpdateImageContent( _lightData, (uint)radiusPixelSize * 2, (uint)radiusPixelSize * 2 );

		_shadowMap.RenderToRt();
		_shadowRenderFullscreen.SetTexture( "_BlitTexture", _lightMap );
		_shadowRenderFullscreen.SetUniform( "resolution", 50 );
		_shadowRenderFullscreen.Draw();
		GlobalVariable.GL.Finish();
		_shadowMap.GetImage( _shadowData );
		var col = Image.TryGetColorPixelRGBA( _shadowData, 359, 0, 360, 1 );
		Console.WriteLine( col );
		
		TexturePool.ReturnTex( _lightMap );
	}

	public override void SetParams( IComponent param ) {
		if( param is ShadowLightComponent globalLightComponent ) {
			_color = globalLightComponent.Color;
			_intensity = globalLightComponent.Intensity;
			_position = globalLightComponent.Position;
			_radius = globalLightComponent.Radius;
			_volume = globalLightComponent.Volume;
			_radialFallOff = globalLightComponent.RadialFallOff;
			_angle = globalLightComponent.Angle;
		}
	}

	public override void Dispose() {

	}
}