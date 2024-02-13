// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information

using System.Numerics;
using Entitas;
using Render;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;


namespace PixelForge {
	class MainLoop {
		static IWindow _window;
		static InputSystem _inputSystem;
		static RenderPipeline _renderPipeline;
		static Vector2 a;

		//static Systems _systems;

		public void Stop() {
			_window.Close();
		}

		static void Main( string[] args ) {
			// var contexts = Contexts.sharedInstance;
			// var e = contexts.game.CreateEntity();
			// e.AddHealth(100);
			//
			// System.Console.WriteLine("e.health.value: " + e.health.Value);
			//
			//
			
			
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
			_renderPipeline = new RenderPipeline( _window );

			//var context = Contexts.sharedInstance;
			//_systems = new Systems().Add(new AddGameSystem(context));
			//_systems.Initialize();


			//设置a的值
			a = new Vector2( 0, 0 );
			VirtualCamera.SetAnchor( "1", ref a );
			VirtualCamera.SetActiveAnchor( "1" );

			// await VirtualCamera.MoveToPos( new Vector2( 1, 1 ), 2000 );
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
			if( InputSystem.GetKey( Key.W ) ) {
				a.Y += 0.1f;
			}
			if( InputSystem.GetKey( Key.S ) ) {
				a.Y -= 0.1f;
			}
			if( InputSystem.GetKey( Key.A ) ) {
				a.X -= 0.1f;
			}
			if( InputSystem.GetKey( Key.D ) ) {
				a.X += 0.1f;
			}
			
			//_systems.Execute();
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