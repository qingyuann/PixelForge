using Entitas;
using PixelForge;
using pp;

namespace Render.PostEffect;

public class BloomComputer : PostProcessComputer {
	RenderFullscreen _gausianBlurHorizontal;
	RenderFullscreen _gaussianBlurVertical;
	RenderFullscreen _bloom;
	RenderTexture tempRT;
	public BloomComputer() {
		_bloom = new RenderFullscreen( "PPBloom.vert", "PPBloom.frag" );
		tempRT = RenderTexturePool.Get( GameSetting.WindowWidth, GameSetting.WindowHeight );
	}

	public override void Render( RenderTexture rt ) {
		Blitter.Blit( rt, tempRT );
		Blitter.Blit( tempRT, rt );
	}

	public override void SetParams( IComponent param ) {

	}
}