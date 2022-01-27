using Roguelike.UI.Actors;
using Roguelike.UI.Infrastructure;
using Roguelike.UI.Interface.Windows;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using Console = System.Console;

namespace Roguelike.UI.Interface;

public class UIManager : ScreenObject
{
    public MapWindow MapWindow { get; set; }
    public MessageLogWindow MessageLog { get; set; }

    public UIManager()
    {
        // must be set to true
        // or will not call each child's Draw method
        IsVisible = true;
        IsFocused = true;

        UseMouse = false;

        // The UIManager becomes the only
        // screen that SadConsole processes
        Parent = GameHost.Instance.Screen;
    }

    public void InitMainMenu()
    {
        /*MainMenu = new MainMenuWindow(GameLoop.GameWidth, GameLoop.GameHeight)
        {
            IsFocused = true
        };
        Children.Add(MainMenu);
        MainMenu.Show();
        MainMenu.Position = new Point(0, 0);*/
        Program.World = new World(Player.TestPlayer(), true);

        CreateMapWindow(Program.GameWidth, Program.GameHeight, "SadConsole Example");
        Children.Add(MapWindow);
        MapWindow.Show();
        MapWindow.IsFocused = true;
        MapWindow.Position = new Point(0, 0);

        IsFocused = true;

        // Then load the map into the MapConsole
        MapWindow.LoadMap(Program.World.CurrentMap);
        MapWindow.CenterOnActor(Program.World.Player);
        
        // Program.UIManager.StartGame(Player.TestPlayer(), true);
        
        System.Console.WriteLine("Console initialized...");
    }
    
    /// <summary>
    /// Scans the SadConsole's Global KeyboardState and triggers behaviour
    /// based on the button pressed.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
    {
        if (SadConsole.GameHost.Instance.Keyboard.IsKeyPressed(Keys.Space))
        {
            System.Console.WriteLine("wtf is going on...");
        }
        
        if (KeyboardHandler.HandleMapKeys(info, this, Program.World))
        {
            System.Console.WriteLine("Keyboard handles something?");
            return true;
        }
        
        return base.ProcessKeyboard(info);
    }
    
    // Creates a window that encloses a map console
    // of a specified height and width
    // and displays a centered window title
    // make sure it is added as a child of the UIManager
    // so it is updated and drawn
    public void CreateMapWindow(int width, int height, string title)
    {
        MapWindow = new MapWindow(width, height, title);

        // The MapWindow becomes a child console of the UIManager
        Children.Add(MapWindow);

        // Add the map console to it
        MapWindow.CreateMapConsole();

        // Without this, the window will never be visible on screen
        MapWindow.Show();
    }
}