using System.Diagnostics;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using Color = SadRogue.Primitives.Color;
using Console = SadConsole.Console;

namespace MyProject
{
    class Program
    {

        public const int Width = 80;
        public const int Height = 25;

        static void Main(string[] args)
        {
            // Setup the engine and creat the main window.
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.Instance.OnStart = Init;
            SadConsole.Game.Instance.FrameUpdate += Instance_FrameUpdate;

            // Start the game.
            SadConsole.Game.Instance.Run();
            //
            // Code here will not run until the game window closes.
            //

            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            // Any custom loading and prep. We will use a sample console for now

            Console startingConsole = new Console(Width, Height);
            startingConsole.DrawBox(new Rectangle(3, 3, 23, 3),
                ShapeParameters.CreateBorder(new ColoredGlyph(Color.Violet, Color.Black, 176)));
            startingConsole.Print(4, 4, "Hello from SadConsole", Color.Aqua);

            // Set our new console as the thing to render and process
            SadConsole.Game.Instance.Screen = startingConsole;
        }

        private static void Instance_FrameUpdate(object sender, GameHost e)
        {
            if (SadConsole.GameHost.Instance.Keyboard.IsKeyPressed(Keys.Space))
            {
                System.Console.Write("space");
            }
        }
    }
}