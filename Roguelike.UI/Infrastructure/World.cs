using Roguelike.UI.Actors;
using Roguelike.UI.Entities;
using Roguelike.UI.Infrastructure.Tiles;
using Roguelike.UI.Infrastructure.Time;
using SadRogue.Primitives;

namespace Roguelike.UI.Infrastructure;

public class World
{
    public Map CurrentMap { get; private set; }
    
    public Player Player { get; set; }
    
    public TimeQueue Time { get; private set; }
    
    // map creation and storage data
    private const int _mapWidth = 50;
    //private const int _mapHeight = 50;
    private readonly int maxChunks;
    private readonly int planetWidth = 257;
    private readonly int planetHeight = 257;
    // private readonly int planetMaxCivs = 30;
    private readonly int _maxRooms = 20;
    private readonly int _minRoomSize = 4;
    private readonly int _maxRoomSize = 10;
    private readonly Random rndNum = new();
    /*private const int _zMaxUpLevel = 10;
    private const int _zMaxLowLevel = -10;*/

    public bool PossibleChangeMap { get; internal set; } = true;

    public SeasonType CurrentSeason { get; set; }

    
    public World(Player player, bool testGame = true)
    {
        Time = new TimeQueue();
        CurrentSeason = SeasonType.Spring;
        
        CreateTestMap();

        PlacePlayer(player);
    }
    
    private void CreateTestMap()
    {
        GeneralMapGenerator generalMapGenerator = new();
        Map map = generalMapGenerator.GenerateTestMap();
        CurrentMap = map;
    }
    
    // Create a player using the Player class
    // and set its starting position
    private void PlacePlayer(Player player)
    {
        // Place the player on the first non-movement-blocking tile on the map
        for (int i = 0; i < CurrentMap.Tiles.Length; i++)
        {
            if (!CurrentMap.Tiles[i].IsBlockingMove && CurrentMap.Tiles[i] is not NodeTile
                                                    && !CurrentMap.GetEntitiesAt<Entity>(Point.FromIndex(i, _mapWidth)).Any())
            {
                // Set the player's position to the index of the current map position
                var pos = Point.FromIndex(i, CurrentMap.Width);

                Player = player;
                Player.Position = pos;
                Player.Description = "Here is you, you are beautiful";
                break;
            }
        }

        // add the player to the Map's collection of Entities
        CurrentMap.Add(Player);
    }
}