using Component;
using Entitas;
using pp;
using System.Numerics;

namespace Render.PostEffect;

public abstract class LightEffectComputer {
	public abstract void Render( RenderTexture rt );
	public abstract void SetParams( List<(ILightComponent,PositionComponent)> param);
	public abstract void Dispose();
}