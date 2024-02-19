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
		// tempRT1 = RenderTexturePool.Get( _halfWidth, _halfHeight );
		tempRT1 = RenderTexturePool.Get( _halfWidth, _halfHeight );
		// tempRT1 = RenderTexturePool.Get( _width, _width );
		tempRT2 = RenderTexturePool.Get( _halfWidth, _halfHeight );
	}

	public override void Render( RenderTexture rt ) {
		//down sample then up sample
		_gausianBlurHorizontal.SetUniform( "offset", _offset );
		_gaussianBlurVertical.SetUniform( "offset", _offset );
		

		// Blitter.Blit( rt, tempRT1);
		// Blitter.Blit( tempRT1, rt);
		// _gausianBlurHorizontal.SetUniform( "screenWidth", _halfWidth );
		// _gausianBlurHorizontal.SetUniform( "screenHeight", _halfHeight );
		// Blitter.Blit( tempRT1, tempRT2, _gausianBlurHorizontal );
		// _gaussianBlurVertical.SetUniform( "screenWidth", _halfWidth );
		// _gaussianBlurVertical.SetUniform( "screenHeight", _halfHeight );
		// Blitter.Blit( tempRT2, tempRT1, _gaussianBlurVertical );
		//
		// Blitter.Blit( tempRT1, rt);
		
		/*for( int i = 0; i < _iterations; i++ ) {
			_gausianBlurHorizontal.SetUniform( "screenWidth", _width );
			_gausianBlurHorizontal.SetUniform( "screenHeight", _height );
			Blitter.Blit( rt, tempRT1, _gausianBlurHorizontal );
			 _gaussianBlurVertical.SetUniform( "screenWidth", _width );
			 _gaussianBlurVertical.SetUniform( "screenHeight", _height );
			// Blitter.Blit( tempRT1, tempRT2, _gaussianBlurVertical );
			// _gausianBlurHorizontal.SetUniform( "screenWidth", _halfWidth );
			// _gausianBlurHorizontal.SetUniform( "screenHeight", _halfHeight );
			// Blitter.Blit( tempRT2, tempRT1, _gausianBlurHorizontal );
			_gausianBlurHorizontal.SetUniform( "screenWidth", GameSetting.WindowWidth );
			_gausianBlurHorizontal.SetUniform( "screenHeight", GameSetting.WindowHeight );
			Blitter.Blit( tempRT1, rt, _gaussianBlurVertical );
		}*/
	}

	public override void SetParams( IComponent param ) {
		if( param is GaussianBlurComponent g ) {
			_iterations = g.Iterations;
			_offset = g.Offset;
		}
	}

	public override void Dispose() {
		RenderTexturePool.Release( tempRT1 );
		RenderTexturePool.Release( tempRT2 );
	}

	~GaussianBlurComputer() {
		Dispose();
	}
}