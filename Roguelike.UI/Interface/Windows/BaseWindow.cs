using SadConsole.UI;

namespace Roguelike.UI.Interface.Windows;

// Map 
public class BaseWindow : Window
{
    public BaseWindow(int width, int height) : base(width, height)
    {
        
    }

    public override void Update(TimeSpan timeSpan)
    {
        base.Update(timeSpan);
    }
    
}