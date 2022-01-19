using Roguelike.UI.Actors;
using Roguelike.UI.Infrastructure.Time;

namespace Roguelike.UI.Infrastructure;

public class World
{
    public Map CurrentMap { get; private set; }
    
    public Player Player { get; set; }
    
    public TimeQueue Time { get; private set; }

    public World()
    {
        
    }
}