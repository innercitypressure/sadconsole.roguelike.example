using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using SadRogue.Primitives;
using Color = SadRogue.Primitives.Color;
using Console = SadConsole.Console;
using Game = SadConsole.Host.Game;

namespace MyProject
{
    class Program
    {

        public const int Width = 80;
        public const int Height = 25;
        private static SadConsole.Entities.Entity player;
        
        static void Main(string[] args)
        {
            // Setup the engine and creat the main window.
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.Instance.OnStart = Init;
            SadConsole.Game.Instance.FrameUpdate += Instance_FrameUpdate;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            Console startingConsole = new Console(Width, Height);
            startingConsole.DrawBox(new Rectangle(3, 3, 23, 3),
                 ShapeParameters.CreateBorder(new ColoredGlyph(Color.Violet, Color.Black, 176)));
             startingConsole.Print(4, 4, "Hello from SadConsole", Color.Aqua);
             
             CreatePlayer();
            
            startingConsole.Children.Add(player);

            foreach (var child in startingConsole.Children)
            {   
                System.Console.WriteLine(child.IsVisible);
                System.Console.WriteLine(child.IsEnabled);
                System.Console.WriteLine(child.Position);
            }
            
            SadConsole.Game.Instance.Screen = startingConsole;
        }

        private static void Instance_FrameUpdate(object sender, GameHost e)
        {
            if (SadConsole.GameHost.Instance.Keyboard.IsKeyPressed(Keys.Space))
            {
                System.Console.Write("space");
            }
        }
        
        // Create a player using SadConsole's Entity class
        private static void CreatePlayer()
        {
            player = new Entity(Color.Green, Color.Green, '@', 0);
            player.Position = new Point(1, 1);
            // player.Appearance.GlyphCharacter = '@';
            player.Appearance.IsVisible = true;
            player.IsEnabled = true;
            player.IsVisible = true;
        }
    }
}