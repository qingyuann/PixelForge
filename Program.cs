// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information

using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace PixelForge
{
    class MainLoop
    {
        private static IWindow _window;
        private static InputSystem _inputSystem;
        
        public void Stop()
        {
            _window.Close();
        }

        static void Main(string[] args)
        {
            WindowOptions options = WindowOptions.Default with
            {
                Size = new Vector2D<int>(800, 600),
                Title = "My first Silk.NET application!"
            };

            _window = Window.Create(options);
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            _window.Run();
        }


        private static void OnLoad()
        {
            void bDown(IKeyboard keyboard, Key key, int i)
            {
                if( key  == Key.B ) {
                    Console.WriteLine("b is down");
                }
            }
            void bUp(IKeyboard keyboard, Key key, int i)
            {
                if( key  == Key.B ) {
                    Console.WriteLine("b is up");
                }
            }
            
            _inputSystem = new InputSystem(ref _window);
            InputSystem.AddKeyDownEvent( bDown );
            InputSystem.AddKeyUpEvent( bUp );
        }
        
        private static void OnUpdate(double deltaTime)
        {
            //在update的最开始执行
            EarlyUpdate(deltaTime);
            //在update的中间执行
            Update(deltaTime);
            //在update的最后执行
            LateUpdate(deltaTime);
        }
        
        private static void EarlyUpdate(double deltaTime)
        {
            _inputSystem.CheckKeys();
        }
        
        private static void Update(double deltaTime)
        {
            if( InputSystem.GetKey( Key.A ) ) {
                Console.WriteLine( "A is pressed!" );
            }
            if( InputSystem.GetKeyDown( Key.A ) ) {
                Console.WriteLine( "A is pressed down!" );
            }
            if( InputSystem.GetKeyUp( Key.A ) ) {
                Console.WriteLine( "A is pressed up!" );
            }
        }
 
        
        private static void LateUpdate(double deltaTime)
        {
            _inputSystem.Clear();
        }
        
        private static void OnRender(double deltaTime)
        {
        }

        private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
        {
            if (key == Key.Escape)
                _window.Close();
            Console.WriteLine($"Key {key} was pressed!");
        }
    }
}