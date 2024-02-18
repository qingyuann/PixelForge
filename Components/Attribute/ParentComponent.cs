using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Component;

[Game]
public class ParentComponent: IComponent{
	[EntityIndex]
	public string ParentName;
}