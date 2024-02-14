using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Component; 

[Game]
public class NameComponent  : IComponent{
	[PrimaryEntityIndex ]
	public string Name;
}