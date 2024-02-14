using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace PixelForge; 

public static class GlobalVariable {
	public static GL Gl;
	public static Contexts Contexts;

	public static void Init(GL gl, Contexts contexts) {
		Gl = gl;
		Contexts = contexts;
	}
}