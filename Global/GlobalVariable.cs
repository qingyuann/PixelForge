using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace PixelForge;

public static class GlobalVariable {
	public static GL GL;
	public static Contexts Contexts;
	public static float XUnit = 1000 / (float)GameSetting.WindowWidth;
	public static float YUnit = 1000 / (float)GameSetting.WindowHeight;
	public static void Init( ref GL gl, ref Contexts contexts ) {
		GL = gl;
		Contexts = contexts;
	}
}