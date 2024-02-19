using Entitas;
using PixelForge;
using pp;

namespace Render.PostEffect;

public class GaussianBlurComputer : PostProcessComputer {
	RenderFullscreen _gausianBlurHorizontal;
	RenderFullscreen _gaussianBlurVertical;
	RenderTexture tempRT1;
	RenderTexture tempRT2;
	int _iterations;
	float _offset;
	static RenderFullscreen _renderFullscreen;
	readonly uint _width = GameSetting.WindowWidth;
	readonly uint _height = GameSetting.WindowHeight;
	readonly uint _halfWidth = GameSetting.WindowWidth / 2;
	readonly uint _halfHeight = GameSetting.WindowHeight / 2;
	readonly uint _quarterWidth = GameSetting.WindowWidth / 4;
	readonly uint _quarterHeight = GameSetting.WindowHeight / 4;
	public GaussianBlurComputer() {
		_gausianBlurHorizontal = new RenderFullscreen( "Blit.vert", "PPGaussianBlurHor.frag" );
		_gaussianBlurVertical = new RenderFullscreen( "Blit.vert", "PPGaussianBlurVer.frag" );
		_renderFullscreen = new RenderFullscreen( "Blit.vert", "Blit.frag" );
	}

	public override void Render( RenderTexture rt ) {
		tempRT1 = RenderTexturePool.Get( _halfWidth, _halfWidth );
		tempRT2 = RenderTexturePool.Get( _halfWidth, _halfWidth );
		_gausianBlurHorizontal.SetUniform( "screenWidth", _halfWidth );
		_gausianBlurHorizontal.SetUniform( "screenHeight", _halfHeight );
		_gaussianBlurVertical.SetUniform( "screenWidth", _halfWidth );
		_gaussianBlurVertical.SetUniform( "screenHeight", _halfHeight );
		_gausianBlurHorizontal.SetUniform( "offset", _offset );
		_gaussianBlurVertical.SetUniform( "offset", _offset );
		Blitter.Blit( rt, tempRT2);
		Blitter.Blit( tempRT2, tempRT1, _gausianBlurHorizontal );
		Blitter.Blit( tempRT1, tempRT2, _gaussianBlurVertical );
		Blitter.Blit( tempRT1, rt);
		
		return;
		
		
		// tempRT1 = RenderTexturePool.Get( _halfWidth, _halfHeight );
		// tempRT2 = RenderTexturePool.Get( _quarterWidth, _quarterHeight );
		//
		// //down sample then up sample
		// _gausianBlurHorizontal.SetUniform( "offset", _offset );
		// _gaussianBlurVertical.SetUniform( "offset", _offset );

		// // half size
		// _gausianBlurHorizontal.SetUniform( "screenWidth", _halfWidth );
		// _gausianBlurHorizontal.SetUniform( "screenHeight", _halfHeight );
		// _gaussianBlurVertical.SetUniform( "screenWidth", _halfWidth );
		// _gaussianBlurVertical.SetUniform( "screenHeight", _halfHeight );
		// Blitter.Blit( rt, tempRT1 );
		// Blitter.Blit( tempRT1, tempRT2, _gausianBlurHorizontal );
		// Blitter.Blit( tempRT2, tempRT1, _gaussianBlurVertical );
		//
		// // quarter size
		// _gausianBlurHorizontal.SetUniform( "screenWidth", _quarterWidth );
		// _gausianBlurHorizontal.SetUniform( "screenHeight", _quarterHeight );
		// _gaussianBlurVertical.SetUniform( "screenWidth", _quarterWidth );
		// _gaussianBlurVertical.SetUniform( "screenHeight", _quarterHeight );
		// RenderTexturePool.Return( tempRT2 );
		// tempRT2 = RenderTexturePool.Get( _quarterWidth, _quarterHeight );
		// Blitter.Blit( tempRT1, tempRT2 );
		// RenderTexturePool.Return( tempRT1 );
		// tempRT1 = RenderTexturePool.Get( _quarterWidth, _quarterHeight );
		// for( int i = 0; i < _iterations; i++ ) {
		// 	Blitter.Blit( tempRT2, tempRT1, _gausianBlurHorizontal );
		// 	Blitter.Blit( tempRT1, tempRT2, _gaussianBlurVertical );
		// }
		//
		// //half size
		// _gausianBlurHorizontal.SetUniform( "screenWidth", _halfWidth );
		// _gausianBlurHorizontal.SetUniform( "screenHeight", _halfHeight );
		// _gaussianBlurVertical.SetUniform( "screenWidth", _halfWidth );
		// _gaussianBlurVertical.SetUniform( "screenHeight", _halfHeight );
		// RenderTexturePool.Return( tempRT1 );
		// tempRT1 = RenderTexturePool.Get( _halfWidth, _halfHeight );
		// Blitter.Blit( tempRT2, tempRT1 );
		// RenderTexturePool.Return( tempRT2 );
		// tempRT2 = RenderTexturePool.Get( _halfWidth, _halfHeight );
		// Blitter.Blit( tempRT1, tempRT2, _gausianBlurHorizontal );
		// Blitter.Blit( tempRT2, tempRT1, _gaussianBlurVertical );
		//
		// //original size
		// Blitter.Blit( tempRT1, rt );
		//
		// RenderTexturePool.Return( tempRT1 );
		// RenderTexturePool.Return( tempRT2 );
	}

	public override void SetParams( IComponent param ) {
		if( param is GaussianBlurComponent g ) {
			_iterations = g.Iterations;
			_offset = g.Offset;
		}
	}

	public override void Dispose() {
		RenderTexturePool.Return( tempRT1 );
		RenderTexturePool.Return( tempRT2 );
	}

	~GaussianBlurComputer() {
		Dispose();
	}
}