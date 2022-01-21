using Roguelike.UI.Interface.Windows;
using SadConsole;
using SadRogue.Primitives;

namespace Roguelike.UI.Interface;

public class UIManager : ScreenObject
{
    public MapWindow MapWindow { get; set; }
    
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
        MapWindow = new MapWindow(Program.GameWidth, Program.GameHeight, "SadConsole Example");
        Children.Add(MapWindow);
        MapWindow.Show();
        MapWindow.Position = new Point(0, 0);
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