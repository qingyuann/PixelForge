using Entitas;
using Silk.NET.OpenGL;
using System.Numerics;
using Texture = Render.Texture;

namespace Mat;

[Game]
public class ParaComponent : IComponent {
	public Dictionary<string, object>? ParaDict;//INT,FLOAT, VECTOR2, VECTOR3, VECTOR4, STRING
	/// <summary>
	/// object 为 Texture或者string
	/// </summary>
	public Dictionary<string, object>? TextureDict;//TEXTURE
}