using Entitas;
using Entitas.CodeGeneration.Attributes;
using Render;
using System.Numerics;

namespace Mat; 
[Game]
public class RenderSingleTriggerComponent  : IComponent{
	public bool IsVisible=true;
	public int Layer;
}