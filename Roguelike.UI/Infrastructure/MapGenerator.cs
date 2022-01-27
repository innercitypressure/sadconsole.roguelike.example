using Roguelike.UI.Infrastructure.Tiles;
using SadRogue.Primitives.GridViews;

namespace Roguelike.UI.Infrastructure;

public abstract class MapGenerator
{
    protected readonly Random randNum = new();
    protected Map _map; // Temporarily store the map currently worked on

    // Empty constructor
    public MapGenerator()
    {
    }

    public MapGenerator(Map map) => _map = map;

    protected void PrepareForFloors()
    {
        foreach (var pos in _map.Positions())
        {
            _map.SetTerrain(new Floor(pos, "stone"));
        }
    }
    
    protected void PrepareForOuterWalls()
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

public class GeneralMapGenerator : MapGenerator
{
    public GeneralMapGenerator()
    {
        // empty
    }

    public Map GenerateTestMap()
    {
        _map = new Map("WTF KEYBOARD");

        PrepareForFloors();
        PrepareForOuterWalls();

        return _map;
    }

    public Map GenerateStoneFloorMap()
    {
        _map = new Map("Test Stone Map");

        PrepareForFloors();

        return _map;
    }
}

public enum RoomTag
{
    Generic, Inn, Temple, Blacksmith, Clothier, Alchemist, PlayerHouse, Hovel, Abandoned
}
