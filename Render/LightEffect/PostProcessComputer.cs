using Entitas;

namespace Render.PostEffect;

public abstract class LightEffectComputer {
	public abstract void Render( RenderTexture rt );
	public abstract void SetParams( IComponent param );
	public abstract void Dispose();
}