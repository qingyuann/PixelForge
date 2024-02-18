namespace PixelForge;

public static class GameSetting {
	public const int WindowWidth = 2048;
	public const int WindowHeight = 2048;
	public const string Name = "PixelForge";
	//public const string ProjectPath = @"./";
	public const string ProjectPath = @"../../../";
	public const int MaxInstancePerDrawCall = 50;
	public const int MaxRenderLayer = 3;

	/// <summary>
	/// 每帧获取deltaTime,毫秒
	/// </summary>
	public static int DeltaTime {
		get;
		private set;
	}

	public static void Load() {
		//加载资源
		DeltaTime = 10;
	}

	public static void Update( double deltaTime ) {
		DeltaTime = (int)( deltaTime * 1000 );
	}
}