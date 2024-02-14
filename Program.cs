// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information

using System.Numerics;
using Render;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Entitas;

namespace PixelForge {
	class MainLoop {
		static IWindow _window;
		static InputSystem _inputSystem;
		static RenderPipeline _renderPipeline;
		static Systems _systems = new Systems();
		static Contexts _contexts = new Contexts();
	
		public void Stop() {
			_window.Close();
		}

		static void Main( string[] args ) {
			GlobalVariable.Contexts = _contexts;
			WindowOptions options = WindowOptions.Default with{
				Size = new Vector2D<int>( GameSetting.WindowWidth, GameSetting.WindowHeight ),
				Title = GameSetting.Name
			};

			_window = Window.Create( options );
			_window.Load += OnLoad;
			_window.Update += OnUpdate;
			_window.Render += OnRender;
			_window.Closing += OnClose;
			_window.Run();
			_window.Dispose();
		}


		static async void OnLoad() {
			GameSetting.Load();
			_inputSystem = new InputSystem( _window );
			_renderPipeline = new RenderPipeline( _window);
			_systems.Add(new AddGameSystem(_contexts));
			_systems.Initialize();
		}

		static void OnUpdate( double deltaTime ) {
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