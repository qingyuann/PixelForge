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
		//string filePath = path is null ? "" : path + "/" + fileName;
		string filePath = path is null ? "" : path + "\\" + fileName;
		return filePath; // 如果path为空，则返回空字符串
	}
}