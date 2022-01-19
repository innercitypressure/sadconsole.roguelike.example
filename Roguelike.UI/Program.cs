using SadConsole;
using SadConsole.Input;

namespace Roguelike.UI
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;

        public static MainScreen mainScreen;
        
        static void Main(string[] args)
        {
            // Setup the engine and creat the main window.
            SadConsole.Game.Create(Width, Height);
            SadConsole.Game.Instance.OnStart = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            mainScreen = new MainScreen {UseKeyboard = true};
            GameHost.Instance.Screen = mainScreen;
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