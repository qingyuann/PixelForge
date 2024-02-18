using Sherlog;
using System.Diagnostics;
using System.Numerics;

namespace PixelForge;

public static class Debug {
	public static void Log( string message ) {
		StackFrame frame = new StackFrame(1, true);
		var lineNumber = frame.GetFileLineNumber();
		var fileName = frame.GetFileName();
        
		Console.WriteLine($"{message} (Debug info: {fileName}, line {lineNumber})");
	}	
	
	public static void Log(Vector3 vector) {
		StackFrame frame = new StackFrame(1, true);
		var lineNumber = frame.GetFileLineNumber();
		var fileName = frame.GetFileName();
		Console.WriteLine($" ({vector.X}, {vector.Y}, {vector.Z})  (Debug info: {fileName}, line {lineNumber})");
	}
	
	public static void Log(Vector2 vector) {
		StackFrame frame = new StackFrame(1, true);
		var lineNumber = frame.GetFileLineNumber();
		var fileName = frame.GetFileName();
		Console.WriteLine($" ({vector.X}, {vector.Y})  (Debug info: {fileName}, line {lineNumber})");
	}
	
	public static void Log(float f) {
		StackFrame frame = new StackFrame(1, true);
		var lineNumber = frame.GetFileLineNumber();
		var fileName = frame.GetFileName();
		Console.WriteLine($" ({f})  (Debug info: {fileName}, line {lineNumber})");
	}
	
	public static void LogError( string message ) {
		StackFrame frame = new StackFrame(1, true);
		var lineNumber = frame.GetFileLineNumber();
		var fileName = frame.GetFileName();

		throw new Exception($"{message} (Debug info: {fileName}, line {lineNumber})");
	}	
}