using GoRogue;
using Roguelike.UI.Infrastructure.Tiles;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace Roguelike.UI.Infrastructure;

public class MapGenerator
{
    private readonly Random random = new();
    
    // Empty constructor
    public MapGenerator()
    {
    }
    
    private Map _map; // Temporarily store the map currently worked on
    
    public Map GenerateTestMap()
    {
        // _map = new Map("Test Map", 50, 30, 3, Distance.Euclidean, 3);

        PrepareForFloors();
        PrepareForOuterWalls();

        return _map;
    }
    
    private void PrepareForFloors()
    {
        foreach (var pos in _map.Positions())
        {
            _map.SetTerrain(new Floor(pos, "stone"));
        }
    }
    
    private void PrepareForOuterWalls()
    {
        foreach (var pos in _map.Positions())
        {
            if (pos.X == 0 || pos.Y == 0 || pos.X == _map.Width - 1 || pos.Y == _map.Height - 1)
            {
                _map.SetTerrain(new Wall(pos, "stone"));
            }
        }
    }
}