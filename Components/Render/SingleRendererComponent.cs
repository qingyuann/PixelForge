using Entitas;
using Render;

namespace Component; 

[Game]
public class SingleRendererComponent  : IComponent{
	public RenderQuad Renderer;
}
