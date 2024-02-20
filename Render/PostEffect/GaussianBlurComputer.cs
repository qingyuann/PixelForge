using Entitas;
using PixelForge;
using pp;

namespace Render.PostEffect;

public class GaussianBlurComputer : PostProcessComputer {
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
	readonly RenderFullscreen _gaussianBlurHorizontal;
	public GaussianBlurComputer() {
		_gaussianBlurVertical = new RenderFullscreen( "Blit_CustomUVScale.vert", "PPGaussianBlurVer.frag" );
		_gaussianBlurHorizontal = new RenderFullscreen( "Blit_CustomUVScale.vert", "PPGaussianBlurHor.frag" );
		_renderFullscreen = new RenderFullscreen( "Blit.vert", "Blit.frag" );
	}

	public override void Render( RenderTexture rt ) {
		//使用opengl自带的blit不会导致纹理坐标变化，但是使用自己的shader去blit半尺寸的纹理时，需要手动将uv坐标放大一倍（0-0.5 -> 0-1）
		_gaussianBlurVertical.SetUniform( "offset", _offset );
		_gaussianBlurHorizontal.SetUniform( "offset", _offset );

		if( _iterations > 0 ) {
			tempRT1 = RenderTexturePool.Get( _halfWidth, _halfHeight );
			tempRT2 = RenderTexturePool.Get( _halfWidth, _halfHeight );
			_gaussianBlurVertical.SetUniform( "screenWidth", _halfWidth );
			_gaussianBlurVertical.SetUniform( "screenHeight", _halfHeight );
			_gaussianBlurVertical.SetUniform( "_UVScale", 1 );
			_gaussianBlurHorizontal.SetUniform( "screenWidth", _halfWidth );
			_gaussianBlurHorizontal.SetUniform( "screenHeight", _halfHeight );
			_gaussianBlurHorizontal.SetUniform( "_UVScale", 1 );
			Blitter.Blit( tempRT1, tempRT2, _gaussianBlurHorizontal );
			Blitter.Blit( tempRT2, tempRT1, _gaussianBlurVertical );
			
			if( _iterations > 1 ) {
				//half size
				RenderTexturePool.Return( tempRT2 );
				tempRT2 = RenderTexturePool.Get( _halfWidth, _halfHeight );
				Blitter.Blit( tempRT1, tempRT2 );
				RenderTexturePool.Return( tempRT1 );
				tempRT1 = RenderTexturePool.Get( _halfWidth, _halfHeight );
	
				_gaussianBlurVertical.SetUniform( "screenWidth", _halfWidth );
				_gaussianBlurVertical.SetUniform( "screenHeight", _halfHeight );
				_gaussianBlurVertical.SetUniform( "_UVScale", 2 );
				_gaussianBlurHorizontal.SetUniform( "screenWidth", _halfWidth );
				_gaussianBlurHorizontal.SetUniform( "screenHeight", _halfHeight );
				_gaussianBlurHorizontal.SetUniform( "_UVScale", 2 );
				Blitter.Blit( tempRT2, tempRT1, _gaussianBlurVertical );
				Blitter.Blit( tempRT1, tempRT2, _gaussianBlurHorizontal );
				Blitter.Blit( tempRT2, tempRT1);

				if( _iterations > 2 ) {
					for( int i = 0; i < _iterations; i++ ) {
						// quarter size
						RenderTexturePool.Return( tempRT2 );
						tempRT2 = RenderTexturePool.Get( _quarterWidth, _quarterHeight );
						Blitter.Blit( tempRT1, tempRT2 );
						RenderTexturePool.Return( tempRT1 );
						tempRT1 = RenderTexturePool.Get( _quarterWidth, _quarterHeight );

						_gaussianBlurVertical.SetUniform( "screenWidth", _quarterWidth );
						_gaussianBlurVertical.SetUniform( "screenHeight", _quarterHeight );
						_gaussianBlurVertical.SetUniform( "_UVScale", 4 );
						_gaussianBlurHorizontal.SetUniform( "screenWidth", _quarterWidth );
						_gaussianBlurHorizontal.SetUniform( "screenHeight", _quarterHeight );
						_gaussianBlurHorizontal.SetUniform( "_UVScale", 4 );
						Blitter.Blit( tempRT2, tempRT1, _gaussianBlurVertical );
						Blitter.Blit( tempRT1, tempRT2, _gaussianBlurHorizontal );

						// half size
						RenderTexturePool.Return( tempRT1 );
						tempRT1 = RenderTexturePool.Get( _halfWidth, _halfHeight );
						Blitter.Blit( tempRT2, tempRT1 );
						RenderTexturePool.Return( tempRT2 );
						tempRT2 = RenderTexturePool.Get( _halfWidth, _halfHeight );

						_gaussianBlurVertical.SetUniform( "screenWidth", _halfWidth );
						_gaussianBlurVertical.SetUniform( "screenHeight", _halfHeight );
						_gaussianBlurVertical.SetUniform( "_UVScale", 2 );
						_gaussianBlurHorizontal.SetUniform( "screenWidth", _halfWidth );
						_gaussianBlurHorizontal.SetUniform( "screenHeight", _halfHeight );
						_gaussianBlurHorizontal.SetUniform( "_UVScale", 2 );
						Blitter.Blit( tempRT1, tempRT2, _gaussianBlurHorizontal );
						Blitter.Blit( tempRT2, tempRT1, _gaussianBlurVertical );
					}
				}
			}
			Blitter.Blit( tempRT1, rt );
			RenderTexturePool.Return( tempRT1 );
			RenderTexturePool.Return( tempRT2 );

			return;
		}

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