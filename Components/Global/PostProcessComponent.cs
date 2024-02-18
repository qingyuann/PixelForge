using Entitas;
using Entitas.CodeGeneration.Attributes;
using Render.PostEffect;

namespace Global;

[Game]
public class PostProcessGroupComponent : IComponent{
	public bool Enabled;
	public int[] Layers;
	public float Intensity;
	public PostProcessComputer[] Computer;
}