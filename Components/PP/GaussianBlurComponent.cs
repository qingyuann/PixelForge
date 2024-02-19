using Entitas;
using Entitas.CodeGeneration.Attributes;
using Render.PostEffect;

namespace pp;

[Game]
public class  GaussianBlurComponent : IComponent,IPostProcessingComponent{
	public bool Enabled { get; set; }
	public int[] Layers { get; set; }
	public float Offset;
	public int Iterations;
	public GaussianBlurComputer Computer;
}