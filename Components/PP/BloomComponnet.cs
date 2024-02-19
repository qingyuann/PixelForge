using Entitas;
using Entitas.CodeGeneration.Attributes;
using Render.PostEffect;

namespace pp;

[Game]
public class BloomComponnet : IComponent{
	public bool Enabled;
	public int[] Layers;
	public float Intensity;
	public BloomComputer Computer;
}