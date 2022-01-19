using GoRogue;
using SadRogue.Primitives;

namespace Roguelike.UI.Infrastructure.Tiles;

public class Floor : BaseTile
{
    public Floor(Coord position, string idMaterial = "stone", bool blocksMove = false, bool tileIsTransparent = true) : base(Color.DarkGray, Color.Transparent, '.', (int)Map.MapLayer.TERRAIN, position, idMaterial, blocksMove, tileIsTransparent)
    {
        Name = "Stone Floor";
    }

    /// <summary>
    /// Constructor for any kind of floor.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="position"></param>
    /// <param name="idMaterial"></param>
    /// <param name="glyph"></param>
    /// <param name="foreground"></param>
    /// <param name="background"></param>
    /// <param name="blocksMove"></param>
    /// <param name="tileIsTransparent"></param>
    public Floor(string name, Coord position, string idMaterial, int glyph, Color foreground, Color background,
        bool blocksMove = false, bool tileIsTransparent = true)
        : base(foreground, background, glyph, (int)Map.MapLayer.TERRAIN, position, idMaterial, blocksMove,
            tileIsTransparent, name)
    {
    }
}