using Silk.NET.Input;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using System.Numerics;

namespace PixelForge;

public class InputSystem {
	private static IInputContext inputContext;

	//按下按键时，将按键添加到这个列表中
	static List<Key> _addWhenKeyDown = new List<Key>();
	static List<Key> _addWhenKeyUp = new List<Key>();
	//每一帧按下的按键
	static List<Key> _keyDown = new List<Key>();
	static List<Key> _keyUp = new List<Key>();
	//管理所有action
	List<KeyValuePair<KeyValuePair<Key, Action>, Action<Key>>> _keyDownActions = new List<KeyValuePair<KeyValuePair<Key, Action>, Action<Key>>>();

	public InputSystem( IWindow _window ) {
		//新建一个keyboard
		inputContext = _window.CreateInput();
		//注册按键，实现按下和抬起的检测
		RegisterKeys();
	}

	/******处理keyDown和keyUp******/
	/// <summary>
	/// 在每帧开始的时候读取按下和抬起的按键
	/// </summary>
	public void CheckKeys() {
		foreach( var key in _addWhenKeyDown ) {
			if( !_keyDown.Contains( key ) ) {
				_keyDown.Add( key );
			}
		}
		foreach( var key in _addWhenKeyUp ) {
			if( !_keyUp.Contains( key ) ) {
				_keyUp.Add( key );
			}
		}
	}

	/// <summary>
	/// 在每帧的最后清空按键读取到的按键，也就是每次点击至少持续一帧
	/// </summary>
	public void Clear() {
		foreach( var key in _keyDown ) {
			_addWhenKeyDown.Remove( key );
		}
		foreach( var key in _keyUp ) {
			_addWhenKeyUp.Remove( key );
		}
		_keyDown.Clear();
		_keyUp.Clear();
	}

	/// <summary>
	/// 当按键按下时，将按键添加到_addWhenKeyDown列表中
	/// </summary>
	void RegisterKeys() {
		for( int i = 0; i < inputContext.Keyboards.Count; i++ ) {
			inputContext.Keyboards[i].KeyDown += ( keyboard, key, keyCode ) => {
				_addWhenKeyDown.Add( key );
			};
			inputContext.Keyboards[i].KeyUp += ( keyboard, key, keyCode ) => {
				_addWhenKeyUp.Add( key );
			};
		}
	}


	/******对外接口******/
	/// <summary>
	/// 当按键按下时，返回一次true
	/// </summary>
	public static bool GetKeyDown( Key key ) {
		return _keyDown.Contains( key );
	}

	/// <summary>
	/// 当按键抬起时，返回一次true
	/// </summary>
	public static bool GetKeyUp( Key key ) {
		return _keyUp.Contains( key );
	}

	/// <summary>
	/// 当按键按下时，持续返回true
	/// </summary>
	public static bool GetKey( Key key ) {
		for( int i = 0; i < inputContext.Keyboards.Count; i++ ) {
			if( inputContext.Keyboards[i].IsKeyPressed( key ) ) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 添加按键按下的方法
	/// </summary>
	public static void AddKeyDownEvent( Action<IKeyboard, Key, int> action ) {
		foreach( IKeyboard t in inputContext.Keyboards ) {
			t.KeyDown += action;
		}
	}

	/// <summary>
	/// 删除按键按下的方法
	/// </summary>
	public static void RemoveKeyDownEvent( Action<IKeyboard, Key, int> action ) {
		foreach( IKeyboard t in inputContext.Keyboards ) {
			t.KeyDown -= action;
		}
	}


	/// <summary>
	/// 添加按键抬起的方法
	/// </summary>
	public static void AddKeyUpEvent( Action<IKeyboard, Key, int> action ) {
		foreach( IKeyboard t in inputContext.Keyboards ) {
			t.KeyUp += action;
		}
	}

	/// <summary>
	///	删除按键抬起的方法
	/// </summary>
	public static void RemoveKeyUpEvent( Action<IKeyboard, Key, int> action ) {
		foreach( IKeyboard t in inputContext.Keyboards ) {
			t.KeyUp -= action;
		}
	}

	/// <summary>
	/// 添加鼠标移动的方法
	/// </summary>
	public static void AddMouseMoveEvent( Action<IMouse, Vector2> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.MouseMove += action;
		}
	}

	/// <summary>
	/// 删除鼠标移动的方法
	/// </summary>
	public static void RemoveMouseMoveEvent( Action<IMouse, Vector2> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.MouseMove -= action;
		}
	}

	/// <summary>
	/// 添加鼠标滚轮滚动的方法
	///  </summary>
	public static void AddMouseScrollEvent( Action<IMouse, ScrollWheel> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.Scroll += action;
		}
	}

	/// <summary>
	/// 删除鼠标滚轮滚动的方法
	/// </summary>
	public static void RemoveMouseScrollEvent( Action<IMouse, ScrollWheel> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.Scroll -= action;
		}
	}

	/// <summary>
	/// 添加鼠标按键按下的方法
	/// </summary>
	public static void AddMouseDownEvent( Action<IMouse, MouseButton> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.MouseDown += action;
		}
	}

	/// <summary>
	/// 删除鼠标按键按下的方法
	/// </summary>
	public static void RemoveMouseDownEvent( Action<IMouse, MouseButton> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.MouseDown -= action;
		}
	}

	/// <summary>
	/// 添加鼠标按键抬起的方法
	/// </summary>
	public static void AddMouseUpEvent( Action<IMouse, MouseButton> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.MouseUp += action;
		}
	}

	/// <summary>
	/// 删除鼠标按键抬起的方法
	/// </summary>
	public static void RemoveMouseUpEvent( Action<IMouse, MouseButton> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.MouseUp -= action;
		}
	}

	/// <summary>
	/// 添加鼠标单击的方法
	/// </summary>
	public static void AddMouseClickEvent( Action<IMouse, MouseButton, Vector2> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.Click += action;
		}
	}

	/// <summary>
	///  删除鼠标单击的方法
	///  </summary>
	public static void RemoveMouseClickEvent( Action<IMouse, MouseButton, Vector2> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.Click -= action;
		}
	}

	/// <summary>
	/// 添加鼠标双击的方法
	///	 </summary>
	public static void AddMouseDoubleClickEvent( Action<IMouse, MouseButton, Vector2> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.DoubleClick += action;
		}
	}
	
	/// <summary>
	/// 删除鼠标双击的方法
	/// </summary>
	public static void RemoveMouseDoubleClickEvent( Action<IMouse, MouseButton, Vector2> action ) {
		foreach( IMouse t in inputContext.Mice ) {
			t.DoubleClick -= action;
		}
	}

}