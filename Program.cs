// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information

using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace PixelForge
{
    class Loop
    {
        private static IWindow _window;

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
            IInputContext input = _window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
                input.Keyboards[i].KeyDown += KeyDown;
        }

        private static void OnUpdate(double deltaTime)
        {
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