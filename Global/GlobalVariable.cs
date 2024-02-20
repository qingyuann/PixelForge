using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace PixelForge; 

public static class GlobalVariable {
	public static GL GL;
	public static Contexts Contexts;

	public static List<byte> SandColor = new List<byte>{ 194, 178, 128, 255 };

	public static void Init(ref GL gl, ref Contexts contexts) {
		GL = gl;
		Contexts = contexts;
	}
}