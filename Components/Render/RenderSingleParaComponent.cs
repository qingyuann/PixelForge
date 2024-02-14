using Entitas;
using Entitas.CodeGeneration.Attributes;
using Render;
using System.Numerics;

namespace Component; 
[Game]
public class RenderSingleParaComponent  : IComponent{
	public bool IsVisible=true;
	public Vector3 Color;
	public string SpriteName="silk.png";
	public int Layer;
}