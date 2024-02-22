using System.Numerics;
using Entitas;
using Light;
using PixelForge;
using PixelForge.Tools;
using pp;

namespace Render.PostEffect;

public class ShadowLightComputer  : LightEffectComputer {
	Vector3 _color;
	float _intensity;
	Vector2 _position;
	float _radius;
	float _volume;
	float _radialFallOff;
	float _angle;
	private RenderTexture _tempRt1;
	byte[] _screenData= Array.Empty<byte>();
	byte[] _lightData= Array.Empty<byte>();
	
	public override void Render( RenderTexture rt ) {
		//get the screen data
		if( _screenData.Length != rt.Width * rt.Height * 4 ) {
			_screenData = new byte[rt.Width * rt.Height * 4];
		}
		rt.GetImage(_screenData);

		//get the light data
		int radiusPixelSize = (int)Transform.WorldToPixelSize( _radius );
		if( _lightData.Length != radiusPixelSize * 2 * radiusPixelSize * 2 * 4) {
			_lightData = new byte[radiusPixelSize * 2 * radiusPixelSize * 2 * 4];
		}
		Vector2 posPixCenter = Transform.WorldToPixel( _position );
		var centerIndex = (int)posPixCenter.X + (int)posPixCenter.Y * rt.Width;

		var color=Image.GetColorPixelRGBA(_screenData, (int)posPixCenter.X, (int)posPixCenter.Y, rt.Width);
		Debug.Log( "color：" +color);
		
		var startIndex = (int)posPixCenter.X - radiusPixelSize + (int)( posPixCenter.Y - radiusPixelSize ) * rt.Width;
		//copy the screen data to light data
		for( int i = 0; i < radiusPixelSize * 2; i++ ) {
			Array.Copy( _screenData, startIndex * 4, _lightData, i * radiusPixelSize * 4, radiusPixelSize * 4 );
			startIndex += rt.Width;
		}
		
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