using SadRogue.Primitives;

namespace Roguelike.UI.Infrastructure.Tiles;

public class NodeTile : BaseTile
{
    public NodeTile(Color foregroud, Color background, int glyph, int layer, Point position, bool blocksMove = true, bool isTransparent = true, string name = "ForgotToChangeName") : base(foregroud, background, glyph, layer, position, blocksMove, isTransparent, name)
    {
    }
}