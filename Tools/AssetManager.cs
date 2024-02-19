namespace PixelForge;

using System;
using System.IO;
public static class AssetManager {
	static Dictionary<string, string> _dictionary = new Dictionary<string, string>();

	static AssetManager() {
		string projectPath = GameSetting.ProjectPath;
		string assetPath = projectPath + "Assets";

		_dictionary = new Dictionary<string, string>();

		// 遍历Asset文件夹下的所有文件
		string[] files = Directory.GetFiles( assetPath, "*", SearchOption.AllDirectories );
		foreach( string filePath in files ) {
			string fileName = Path.GetFileName( filePath ); // 获取文件名
			string directory = Path.GetDirectoryName( filePath ); // 获取文件所在目录
			_dictionary.Add( fileName, directory ); // 将文件名和目录存入字典
		}
	}

	public static string GetAssetPath( string fileName ) {
		_dictionary.TryGetValue( fileName, out string? path );
		PlatformID platform = Environment.OSVersion.Platform;
		string filePath;
		if (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows || platform == PlatformID.WinCE)
		{
			//windows
			filePath = path is null ? "" : path + "\\" + fileName;
		}
		else if (platform == PlatformID.MacOSX || platform == PlatformID.Unix)
		{
			//mac
			filePath = path is null ? "" : path + "/" + fileName;
		}
		else
		{
			filePath = path is null ? "" : path + "\\" + fileName;
			Console.WriteLine("未知操作系统");
		}
		return filePath; // 如果path为空，则返回空字符串
	}
}