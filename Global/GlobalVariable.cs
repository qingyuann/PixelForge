using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using System.Numerics;

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

	/// <summary>
	/// 每帧获取deltaTime,毫秒
	/// </summary>
	public static int DeltaTime {
		get;
		private set;
	} = 10;

	public static Vector4 Time = new Vector4( 0f, 0f, 0f, 0f );


	public static void Load() {
		//加载资源
		DeltaTime = 10;
	}

	public static void Update( double deltaTime ) {
		DeltaTime = (int)( deltaTime * 1000 );
		Time.X += DeltaTime;
		Time.Y = Time.X / 1000;
		Time.Z = Time.Y / 10;
		Time.W = Time.Z * 2;
	}
}