using Entitas;
using Entitas.CodeGeneration.Attributes;
using Render.PostEffect;

namespace pp;

[Game][Unique]
public class LightSettingComponent : IComponent{
	[PrimaryEntityIndex]
	public bool Enabled;
}