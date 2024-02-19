using Entitas;

namespace Render.PostEffect;

public abstract class PostProcessComputer {
	public abstract void Render( RenderTexture rt );
	public abstract void SetParams( IComponent param );
}