using GoRogue;
using SadRogue.Primitives;

namespace Roguelike.UI.Infrastructure.Tiles;

public sealed class Floor : BaseTile
{
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
    public Floor(string name, Point position, string idMaterial, int glyph, Color foreground, Color background,
        bool blocksMove = false, bool tileIsTransparent = true)
        : base(foreground, background, glyph, (int)MapLayer.TERRAIN, position, idMaterial, blocksMove,
            tileIsTransparent, name)
    {
    }

    /// <summary>
    /// Default constructor or a stone tile.
    /// \nFloors are set to allow movement and line of sight by default
    /// and have a dark gray foreground and a transparent background
    /// represented by the . symbol
    /// </summary>
    /// <param name="position"></param>
    /// <param name="blocksMove"></param>
    /// <param name="tileIsTransparent"></param>
    public Floor(Point position, string idMaterial = "stone", bool blocksMove = false, bool tileIsTransparent = true)
        : base(Color.DarkGray, Color.Transparent, '.', (int)MapLayer.TERRAIN, position, idMaterial, blocksMove, tileIsTransparent)
    {
        Name = "Stone Floor";
    }
}