using SadConsole.Entities;
using SadRogue.Primitives;

namespace Roguelike.UI.Entities;

public class Actor : Entity
{
    private bool bumped = false;
    
    public Actor(Color foreground, Color background, int glyph, int zIndex) : base(foreground, background, glyph, zIndex)
    {
    }
}