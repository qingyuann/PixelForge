using Entitas;
using Silk.NET.OpenGL;

namespace Component;


[Game]
public class TextureComponent  : IComponent {
	public string TextureName;
	public Texture? Texture;
	public string? TextureFileName;
}