using Roguelike.UI.Actors;

namespace Roguelike.UI.Infrastucture;

public class World
{
    public Map CurrentMap { get; private set; }
    
    public Player Player { get; set; }
    
    
}