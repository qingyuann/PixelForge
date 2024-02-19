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

	public GaussianBlurComputer() {
		_gausianBlurHorizontal = new RenderFullscreen( "Blit.vert", "PPGaussianBlurHor.frag" );
		_gaussianBlurVertical = new RenderFullscreen( "Blit.vert", "PPGaussianBlurVer.frag" );
		_renderFullscreen = new RenderFullscreen( "Blit.vert", "Blit.frag" );
		tempRT1 = RenderTexturePool.Get( GameSetting.WindowWidth, GameSetting.WindowHeight );
		tempRT2 = RenderTexturePool.Get( GameSetting.WindowWidth, GameSetting.WindowHeight );
	}

	public override void Render( RenderTexture rt ) {
		_gausianBlurHorizontal.SetUniform( "screenWidth", GameSetting.WindowWidth );
		_gausianBlurHorizontal.SetUniform( "screenHeight", GameSetting.WindowHeight );
		_gausianBlurHorizontal.SetUniform( "offset", _offset );
		_gaussianBlurVertical.SetUniform( "screenWidth", GameSetting.WindowWidth );
		_gaussianBlurVertical.SetUniform( "screenHeight", GameSetting.WindowHeight );
		_gaussianBlurVertical.SetUniform( "offset", _offset );
		for( int i = 0; i < _iterations; i++ ) {
			Blitter.Blit( rt, tempRT1 , _gausianBlurHorizontal);
			Blitter.Blit( tempRT1, rt, _gaussianBlurVertical );
		}
	}
	
	public override void SetParams( IComponent param ) {
		if( param is GaussianBlurComponent g ) {
			_iterations = g.Iterations;
			_offset = g.Offset;
		}
	}
	
	public void Dispose() {
		RenderTexturePool.Release( tempRT1 );
		RenderTexturePool.Release( tempRT2 );
	}

	~GaussianBlurComputer() {
		Dispose();
	}
}