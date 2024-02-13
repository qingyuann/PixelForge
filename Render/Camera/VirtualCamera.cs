using PixelForge;
using System.Numerics;
using System.Reflection;

namespace Render;

public struct PosAnchor {
	public readonly string Name;
	public readonly unsafe Vector2* Position;

	public unsafe PosAnchor( string name, ref Vector2 position ) {
		Name = name;
		fixed (Vector2* a = &position) {
			Position = a;
		}
	}
}
public static class VirtualCamera {
	public static Camera _camera = new Camera( new Vector2( 0, 0 ), 0.5f );
	static List<PosAnchor> _anchors = new();
	static int activeAnchor = -1;

	/// <summary>
	/// 平滑移动到pos
	/// </summary>
	/// <param name="pos">目标位置</param>
	/// <param name="time">毫秒</param>
	public static async Task MoveToPos( Vector2 pos, int time ) {
		int startTime = 0;
		Vector2 startPos = _camera.Position;
		while( startTime < time ) {
			var deltaTime = GameSetting.DeltaTime;
			float t = (float)startTime / time;				
			_camera.Position = Vector2.Lerp( startPos, pos, t );
			startTime += deltaTime;
					await Task.Delay( deltaTime );
		}
	}


	/// <summary>
	/// 设置锚点
	/// </summary>
	/// <param name="name"></param>
	/// <param name="pos"></param>
	public static void SetAnchor( string name, ref Vector2 pos )
	{
		int ass = 1;
		ref readonly var a = ref ass;
		//如果重名就覆盖,否则添加
		for( int i = 0; i < _anchors.Count; i++ ) {
			if( _anchors[i].Name == name ) {
				_anchors[i] = new PosAnchor( name, ref pos );
				return;
			}
		}
		_anchors.Add( new PosAnchor( name, ref pos ) );
	}

	/// <summary>
	/// 移除锚点
	/// </summary>
	/// <param name="name"></param>
	public static void RemoveAnchor( string name ) {
		for( int i = 0; i < _anchors.Count; i++ ) {
			if( _anchors[i].Name == name ) {
				_anchors.RemoveAt( i );
				return;
			}
		}
	}


	/// <summary>
	/// 打印所有锚点
	/// </summary>
	public static void LogAnchors() {
		foreach( var anchor in _anchors ) {
			unsafe {
				Console.WriteLine( "Updated Position: " + anchor.Position->X + ", " + anchor.Position->Y );
			}
		}
	}

	/// <summary>
	/// 输入名字，返回锚点位置
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Vector3 GetAnchorPosition( string name ) {
		for( int i = 0; i < _anchors.Count; i++ ) {
			if( _anchors[i].Name == name ) {
				unsafe {
					return new Vector3( _anchors[i].Position->X, _anchors[i].Position->Y, 0 );
				}
			}
		}
		return new Vector3( 0, 0, 0 );
	}

	/// <summary>
	/// 获得当前相机位置
	/// </summary>
	/// <returns></returns>
	public static Vector3 GetCameraPosition() {
		return new Vector3( _camera.Position, 0 );
	}

	/// <summary>
	/// 设置当前锚点
	/// </summary>
	/// <param name="name"></param>
	public static void SetActiveAnchor( string name ) {
		for( int i = 0; i < _anchors.Count; i++ ) {
			if( _anchors[i].Name == name ) {
				activeAnchor = i;
				return;
			}
		}
	}


	/// <summary>
	/// 获取相机矩阵
	/// </summary>
	/// <returns></returns>
	public static Matrix4x4 GetViewMatrix() {
		//如果有锚点就移动到锚点
		if( activeAnchor != -1 ) {
			unsafe {
				_camera.Position.X = _anchors[activeAnchor].Position->X;
				_camera.Position.Y = _anchors[activeAnchor].Position->Y;
			}
		}
		return _camera.GetViewMatrix();
	}
}