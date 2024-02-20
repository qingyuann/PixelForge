using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace PixelForge; 

public static class GlobalVariable {
	public static GL GL;
	public static Contexts Contexts;

	public static void Init(ref GL gl, ref Contexts contexts) {
		GL = gl;
		Contexts = contexts;
	}
}