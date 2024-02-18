namespace Render.PostEffect;

public abstract class PostProcessComputer {
	public abstract void Render( RenderTexture rt );
	public abstract void SetParams( Dictionary<string, object> param );
}