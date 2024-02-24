namespace PixelForge;

public static class GameSetting {
	public const int WindowWidth = 1000;
	public const int WindowHeight = 1000;
	public const string Name = "PixelForge";
	//public const string ProjectPath = @"./";
	public static string ProjectPath
	{
		get
		{
			PlatformID platform = Environment.OSVersion.Platform;
			if (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows || platform == PlatformID.WinCE)
			{
				return @"../../../";
			}
			else
			{
				return @"./";
			}
		}
	}
	public const int MaxInstancePerDrawCall = 50;
	public const int MaxRenderLayer = 3;

	
	
	/// <summary>
	/// 每帧获取deltaTime,毫秒
	/// </summary>
	public static int DeltaTime {
		get;
		private set;
	}=10;

	public static void Load() {
		//加载资源
		DeltaTime = 10;
	}

	public static void Update( double deltaTime ) {
		DeltaTime = (int)( deltaTime * 1000 );
	}
}