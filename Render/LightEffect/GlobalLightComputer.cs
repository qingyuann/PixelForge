using Component;
using System.Numerics;
using Entitas;
using Light;
using PixelForge;
using pp;
using Silk.NET.OpenGL;

namespace Render.PostEffect;

public class GlobalLightComputer : LightEffectComputer {
	List<Vector3> _color = new List<Vector3>();
	List<float> _intensity = new List<float>();
	List<float> _merge = new List<float>();
	RenderFullscreen _globalLightDraw;

	public GlobalLightComputer() {
		_globalLightDraw = new RenderFullscreen( "Blit.vert", "GlobalLightDraw.frag" );
	}
	public override void Render( RenderTexture rt ) {
		RenderTexture tempRt = TexturePool.GetRT( (uint)rt.Width, (uint)rt.Height );
		RenderTexture tempRt1 = TexturePool.GetRT( (uint)rt.Width, (uint)rt.Height );
		Blitter.Blit( rt, tempRt );

		for( int i = 0; i < _color.Count; i++ ) {
			_globalLightDraw.SetUniform( "color", _color[i] );
			_globalLightDraw.SetUniform( "intensity", _intensity[i] );
			_globalLightDraw.SetUniform( "merge", _merge[i] );
			Blitter.Blit( tempRt, tempRt1, _globalLightDraw );
			Blitter.Blit( tempRt1, tempRt );
		}

		/////////////////////////////////
		//// step7: release the data ////
		/////////////////////////////////
		Blitter.Blit( tempRt, rt );
		TexturePool.ReturnRT( tempRt );
		TexturePool.ReturnRT( tempRt1 );
	}

	public override void SetParams( List<(ILightComponent, PositionComponent)> param ) {
		_color.Clear();
		_intensity.Clear();
		_merge.Clear();
		for( int i = 0; i < param.Count; i++ ) {
			if( param[i].Item1 is GlobalLightComponent globalLightComponent ) {
				_color.Add( globalLightComponent.Color );
				_intensity.Add( globalLightComponent.Intensity );
				_merge.Add( globalLightComponent.Merge );
			}
		}
	}

	public override void Dispose() {
	}
}