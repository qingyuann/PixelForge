namespace PixelForge; 

public static class GameSetting {
	public const int WindowWidth = 800;
	public const int WindowHeight = 700;
	public const string Name = "PixelForge";
	public const string ProjectPath = @"../../../";
	
	public const int ChunkSize = 16;
	
	public static DateTime StartTime;

	
	public static void OnLoad() {
		StartTime = DateTime.UtcNow;
	}
	
}