using Entitas;
using Entitas.CodeGeneration.Attributes;
using Render.PostEffect;

namespace pp;

[Game]
public class BloomComponent : IComponent, IPostProcessingComponent {
	public bool Enabled { get; set; }
	public int[] Layers { get; set; }
	public float BlurOffset;
	public int BlurIterations;
	public float BloomThreshold;
	public float BloomIntensity;
	public BloomComputer Computer;
}