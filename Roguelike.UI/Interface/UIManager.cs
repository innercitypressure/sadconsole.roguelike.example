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
    public StatusWindow StatusConsole { get; set; }

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
        Program.World = new World(Player.TestPlayer(), true);
        IsFocused = true;
        InitializeNewGame();
    }

    public void InitializeNewGame()
    {   
        //Message Log initialization
        MessageLog = new MessageLogWindow(Program.GameWidth / 2, Program.GameHeight / 2, "Message Log");
        Children.Add(MessageLog);
        MessageLog.Show();
        MessageLog.Position = new Point(Program.GameWidth / 2, Program.GameHeight / 2);
        MessageLog.Add("Test message log works");
        
        StatusConsole = new StatusWindow(Program.GameWidth / 2, Program.GameHeight / 2, "Status Window");
        Children.Add(StatusConsole);
        StatusConsole.Position = new Point(Program.GameWidth / 2, 0);
        StatusConsole.Show();
        
        // Build the Window
        CreateMapWindow(Program.GameWidth / 2, Program.GameHeight, "Game Map");

        // Then load the map into the MapConsole
        MapWindow.LoadMap(Program.World.CurrentMap);

        // Start the game with the camera focused on the player
        MapWindow.CenterOnActor(Program.World.Player);
    }
    
    /// <summary>
    /// Scans the SadConsole's Global KeyboardState and triggers behaviour
    /// based on the button pressed.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
    {
        if (KeyboardHandler.HandleMapKeys(info, this, Program.World))
        {
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