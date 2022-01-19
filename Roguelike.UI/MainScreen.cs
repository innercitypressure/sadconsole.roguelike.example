using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using SadRogue.Primitives;
using Console = SadConsole.Console;
using Map = Roguelike.UI.Infrastucture.Map;

namespace Roguelike.UI;

public class MainScreen : ScreenObject
{
    private static Map MapScreen { get; set; }
    private static Entity player { get; set; }
    
    private static Renderer renderer { get; set; }
    
    public MainScreen()
    {
        InitMap();
    }

    private void InitMap()
    {
        var console = new Console(80, 25);
        CreatePlayer();
        console.Children.Add(player);
        console.Print(4, 4, "Hello from SadConsole", Color.Aqua);
        player.IsVisible = true;
        console.IsVisible = true;
        console.IsEnabled = true;
        console.IsFocused = true;
        Children.Add(console);
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (SadConsole.GameHost.Instance.Keyboard.IsKeyPressed(Keys.Space))
        {
            System.Console.Write("space");
            return true;
        }

        return false;
    }

    // Create a player using SadConsole's Entity class
    private static void CreatePlayer()
    {
        player = new Entity(Color.Green, Color.Red, '@', 1);
        player.Position = new Point(1, 1);
        // player.Appearance.GlyphCharacter = '@';
        player.Appearance.IsVisible = true;
        player.IsEnabled = true;
        player.IsVisible = true;
    }
}