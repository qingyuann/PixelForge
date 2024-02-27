// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;
using Render;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.GLFW;
using Entitas;
using Silk.NET.OpenGL;

namespace PixelForge {
	class MainLoop {
		static IWindow _window;
		static InputSystem _inputSystem;
		static RenderPipeline _renderPipeline;
		static Systems _systems = new Systems();
		static Contexts _contexts = new Contexts();
		
		private static Stopwatch _stopwatch = new Stopwatch();
		private static int _frameCount = 0;
		private static float _fps = 0;

		public void Stop() {
			_window.Close();
		}

		static void Main( string[] args ) {
			GlobalVariable.Contexts = _contexts;
			WindowOptions options = WindowOptions.Default with{
				Size = new Vector2D<int>( GameSetting.WindowWidth, GameSetting.WindowHeight ),
				Title = GameSetting.Name,
				FramesPerSecond = 120,
				UpdatesPerSecond = 120,
				VSync = true,
				WindowBorder = WindowBorder.Resizable,
				ShouldSwapAutomatically = true
			};

			_window = Window.Create( options );

			_stopwatch.Start();
			
			_window.Load += OnLoad;
			_window.Update += OnUpdate;
			_window.Render += OnRender;
			_window.Closing += OnClose;
			_window.Run();
			_window.Dispose();
		}


		static async void OnLoad() {
			GameSetting.Load();
			GlobalVariable.GL = GL.GetApi( _window );

			_inputSystem = new InputSystem( _window );
			_renderPipeline = new RenderPipeline( GlobalVariable.GL );
			_systems.Add( new AddGameSystem( _contexts ) );
			_systems.Initialize();
		}

		static void OnUpdate( double deltaTime ) {
			_frameCount++;
			if (_stopwatch.ElapsedMilliseconds >= 1000)
			{
				_fps = _frameCount;
				_frameCount = 0;
				_stopwatch.Restart();
			}
			
			_window.Title = $"{GameSetting.Name} - FPS: {_fps}";

			//在update的最开始执行
			EarlyUpdate( deltaTime );
			//在update的中间执行
			Update( deltaTime );
			//在update的最后执行
			LateUpdate( deltaTime );
		}

		static void EarlyUpdate( double deltaTime ) {
			//更新deltaTime
			GameSetting.Update( 0.1 );
			_inputSystem.CheckKeys();
		}

		static void Update( double deltaTime ) {
			_systems.Execute();
		}

		static void LateUpdate( double deltaTime ) {
			_inputSystem.Clear();
		}

		static void OnRender( double deltaTime ) {
			_renderPipeline.OnRender();
		}

		static void OnClose() {
			_renderPipeline.OnClose();
		}
	}
}