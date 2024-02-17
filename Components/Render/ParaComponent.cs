using Entitas;
using Silk.NET.OpenGL;
using System.Numerics;
using Texture = Render.Texture;

namespace Mat;

[Game]
public class ParaComponent : IComponent {
	public Dictionary<string, object>? ParaDict;
	/// <summary>
	/// object 为 Texture或者string
	/// </summary>
	public Dictionary<string, object>? TextureDict;
}