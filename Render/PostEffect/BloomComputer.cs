namespace Render.PostEffect;

public class BloomComputer: PostProcessComputer{
	RenderFullscreen renderFullscreen;
	public BloomComputer( ) {
		renderFullscreen = new RenderFullscreen( );
	}
	public override void Render( RenderTexture rt ) {
		
	}
	public override void SetParams( Dictionary<string, object> param ) {

	}
}