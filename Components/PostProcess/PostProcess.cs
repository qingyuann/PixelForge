using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace PP;

[Unique]
public class PostProcess :IComponent{
	public bool Enabled;
	public int Sequence;
	public List<int> Layers;
	public PostProcess PostProcessEffect;
}