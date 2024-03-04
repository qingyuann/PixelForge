using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;

namespace PixelForge;

public static class GlobalVariable {
	public static GL GL;
	public static Contexts Contexts;
	public static IWindow Window;
	// 1 unit = 1000 pixels
	// world_size_x * XUnit = screen_ratio_x
	public static float UnitPixel = 1000;
	public static float XUnit = UnitPixel / (float)GameSetting.WindowWidth;
	public static float YUnit = UnitPixel / (float)GameSetting.WindowHeight;
	public static void Init( ref GL gl, ref Contexts contexts ) {
		GL = gl;
		Contexts = contexts;
	}
}