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
    
    private void UpdateIfNeedTheMap(Map mapToGo)
    {
        if (mapToGo.NeedsUpdate)
        {
            // do something
        }
        else
            return; // Do nothing
    }
    
    public void ProcessTurn(long playerTime, bool success)
    {
        if (success)
        {
            /*if (Player.Stats.Health <= 0)
            {
                RestartGame();
                return;
            }*/

            PlayerTimeNode playerTurn = new PlayerTimeNode(Time.TimePassed.Ticks + playerTime);
            Time.RegisterEntity(playerTurn);

            // Player.Stats.ApplyHpRegen();
            // Player.Stats.ApplyManaRegen();
            CurrentMap.PlayerFOV.Calculate(Player.Position, 20);

            var node = Time.NextNode();

            while (node is not PlayerTimeNode)
            {
                switch (node)
                {
                    case EntityTimeNode entityTurn:
                        // ProcessAiTurn(entityTurn.EntityId, Time.TimePassed.Ticks);
                        break;

                    default:
                        throw new NotSupportedException($"Unhandled time master node type: {node.GetType()}");
                }

                node = Time.NextNode();
            }

            Program.UIManager.MapWindow.MapConsole.IsDirty = true;
            System.Console.WriteLine("Processing Turns?");

            System.Console.WriteLine($"Turns: {Time.Turns}, Tick: {Time.TimePassed.Ticks}");
           // Program.UIManager.MessageLog.Add($"Turns: {Time.Turns}, Tick: {Time.TimePassed.Ticks}");
        }
    }
    
    /*public bool MapIsWorld()
    {
        if (CurrentMap == WorldMap.AssocietatedMap)
            return true;
        else
            return false;
    }*/
    
    public void ChangeControlledEntity(Entity entity)
    {
        CurrentMap.ControlledEntitiy = entity;
    }
    
    public void ForceChangeCurrentMap(Map map) => CurrentMap = map;
}