using Newtonsoft.Json;
using Roguelike.UI;
using Roguelike.UI.Actors;
using Roguelike.UI.Commands;
using Roguelike.UI.Entities;
using Roguelike.UI.Infrastructure;
using Roguelike.UI.Infrastructure.Tiles;
using Roguelike.UI.Infrastructure.Time;
using Roguelike.UI.Interface;
using SadConsole.Input;
using SadRogue.Primitives;

namespace Roguelike.UI.Interface;

public static class KeyboardHandler
{
    private static Player GetPlayer => Program.World.Player;

    // TODO: Not ready for target cursors just yet
    // private static Target targetCursor;

    private static readonly Dictionary<Keys, Direction> MovementDirectionMapping = new Dictionary<Keys, Direction>
    {
        { Keys.NumPad7, Direction.UpLeft }, { Keys.NumPad8, Direction.Up }, { Keys.NumPad9, Direction.UpRight },
        { Keys.NumPad7, Direction.UpLeft }, { Keys.NumPad8, Direction.Up }, { Keys.NumPad9, Direction.UpRight },
        { Keys.NumPad4, Direction.Left }, { Keys.NumPad6, Direction.Right },
        { Keys.NumPad1, Direction.DownLeft }, { Keys.NumPad2, Direction.Down }, { Keys.NumPad3, Direction.DownRight },
        { Keys.Up, Direction.Up }, { Keys.Down, Direction.Down }, { Keys.Left, Direction.Left },
        { Keys.Right, Direction.Right }
    };

    public static bool HandleMapKeys(Keyboard input, UIManager ui, World world)
    {
        if (HandleActions(input, world, ui))
            return true;

        return false;
    }

    private static bool HandleMove(Keyboard info, World world)
    {
        var console = Program.UIManager.MapWindow.MapConsole;

        var worldmap = false;

        if (worldmap)
        {
            if (info.IsKeyDown(Keys.Left))
            {
                console.ViewPosition = console.ViewPosition.Translate((-1, 0));
            }

            if (info.IsKeyDown(Keys.Right))
            {
                console.ViewPosition = console.ViewPosition.Translate((1, 0));
            }

            if (info.IsKeyDown(Keys.Up))
            {
                console.ViewPosition = console.ViewPosition.Translate((0, -1));
            }

            if (info.IsKeyDown(Keys.Down))
            {
                console.ViewPosition = console.ViewPosition.Translate((0, +1));
            }

            // Must return false, because there isn't any movement of the actor
            return false;
        }

        if (info.IsKeyDown(Keys.F))
        {
            System.Console.WriteLine("Spacfe key");
        }
        
        foreach (Keys key in MovementDirectionMapping.Keys)
        {
            if (info.IsKeyPressed(key) && world.CurrentMap is not null)
            {
                Direction moveDirection = MovementDirectionMapping[key];
                Point coorToMove = new(moveDirection.DeltaX, moveDirection.DeltaY);

                bool success =
                    CommandManager.MoveActorBy((Actor)world.CurrentMap.ControlledEntitiy, coorToMove);
                return success;
            }
        }

        return false;
    }

    private static bool HandleActions(Keyboard info, World world, UIManager ui)
    {
        // Work around for a > symbol, must be top to not make the char wait
        if (info.IsKeyDown(Keys.LeftShift) && info.IsKeyPressed(Keys.OemPeriod))
        {
            Program.UIManager.MessageLog.Add($"Move up..");
            // return CommandManager.EnterDownMovement(GetPlayer.Position);
        }

        // Work around for a < symbol, must be top to not make the char wait
        if (info.IsKeyDown(Keys.LeftShift) && info.IsKeyPressed(Keys.OemComma))
        {
            Program.UIManager.MessageLog.Add($"Move down..");
            // return CommandManager.EnterUpMovement(GetPlayer.Position);
        }

        if (HandleMove(info, world))
        {
            System.Console.WriteLine("Key... ");
            
            world.ProcessTurn(TimeHelper.GetWalkTime(GetPlayer,
                world.CurrentMap.GetTileAt<BaseTile>(GetPlayer.Position)), true);

            /*if (!GetPlayer.Bumped && world.CurrentMap.ControlledEntitiy is Player)
                world.ProcessTurn(TimeQueue.GetWalkTime(GetPlayer,
                    world.CurrentMap.GetTileAt<TileBase>(GetPlayer.Position)), true);
            else if (world.CurrentMap.ControlledEntitiy is Player)
                world.ProcessTurn(TimeQueue.GetAttackTime(GetPlayer), true);
                */
            return true;
        }

        if (info.IsKeyPressed(Keys.NumPad5) && info.IsKeyDown(Keys.LeftControl))
        {
            Program.UIManager.MessageLog.Add($"Rest up..");
            // return CommandManager.RestTillFull(GetPlayer);
        }

        if (info.IsKeyPressed(Keys.NumPad5) || info.IsKeyPressed(Keys.OemPeriod))
        {
            world.ProcessTurn(TimeHelper.Wait, true);
            return true;
        }

        return false;

        /*if (info.IsKeyPressed(Keys.A))
        {
            bool sucess = CommandManager.DirectAttack(world.Player);
            world.ProcessTurn(TimeQueue.GetAttackTime(world.Player), sucess);
            return sucess;
        }
        if (info.IsKeyPressed(Keys.G))
        {
            Item item = world.CurrentMap.GetEntityAt<Item>(world.Player.Position);
            bool sucess = CommandManager.PickUp(world.Player, item);
            ui.InventoryScreen.ShowItems(world.Player);
            world.ProcessTurn(TimeQueue.Interact, sucess);
            return sucess;
        }
        if (info.IsKeyPressed(Keys.D))
        {
            bool sucess = CommandManager.DropItems(world.Player);
            ui.InventoryScreen.ShowItems(world.Player);
            world.ProcessTurn(TimeQueue.Interact, sucess);
            return sucess;
        }
        if (info.IsKeyPressed(Keys.C))
        {
            bool sucess = CommandManager.CloseDoor(world.Player);
            world.ProcessTurn(TimeQueue.Interact, sucess);
            ui.MapWindow.MapConsole.IsDirty = true;
        }
        if (info.IsKeyPressed(Keys.H) && !info.IsKeyDown(Keys.LeftShift))
        {
            bool sucess = CommandManager.SacrificeLifeEnergyToMana(world.Player);
            world.ProcessTurn(TimeQueue.MagicalThings, sucess);
        }
        if (info.IsKeyPressed(Keys.H) && info.IsKeyDown(Keys.LeftShift))
        {
            bool sucess = CommandManager.NodeDrain(GetPlayer);
            world.ProcessTurn(TimeQueue.MagicalThings, sucess);
        }
        if (info.IsKeyPressed(Keys.L))
        {
            if (targetCursor is null)
                targetCursor = new Target(GetPlayer.Position);

            if (world.CurrentMap.ControlledEntitiy is not Player
                && !targetCursor.EntityInTarget())
            {
                targetCursor.EndTargetting();
                return true;
            }

            targetCursor.StartTargetting();
            targetCursor.Cursor.IgnoresWalls = true;

            return true;
        }

        if (info.IsKeyDown(Keys.LeftShift) && info.IsKeyPressed(Keys.Z))
        {
            SpellSelectWindow spell =
                new SpellSelectWindow(GetPlayer.Stats.PersonalMana);

            targetCursor = new Target(GetPlayer.Position);

            spell.Show(GetPlayer.Magic.KnowSpells,
                selectedSpell => targetCursor.OnSelectSpell(selectedSpell,
                (Actor)world.CurrentMap.ControlledEntitiy),
                GetPlayer.Stats.PersonalMana);

            return true;
        }

        if (info.IsKeyPressed(Keys.Enter) && targetCursor is not null
            && targetCursor.State == Target.TargetState.Targeting)
        {
            if (targetCursor.EntityInTarget() && targetCursor.SpellSelected is null)
            {
                targetCursor.LookTarget();
                return true;
            }
            if (targetCursor.EntityInTarget() || targetCursor.SpellTargetsTile())
            {
                var (sucess, spellCasted) = targetCursor.EndSpellTargetting();

                if (sucess)
                {
                    targetCursor = null;
                    world.ProcessTurn(TimeQueue.GetCastingTime(GetPlayer, spellCasted), sucess);
                }
                return sucess;
            }
            else
            {
                ui.MessageLog.Add("Invalid target!");
                return false;
            }
        }

        if (info.IsKeyPressed(Keys.Escape) && (targetCursor is not null))
        {
            targetCursor.EndTargetting();

            targetCursor = null;

            return true;
        }

#if DEBUG
        if (info.IsKeyPressed(Keys.F10))
        {
            CommandManager.ToggleFOV();
            ui.MapWindow.MapConsole.IsDirty = true;
        }

        if (info.IsKeyPressed(Keys.F8))
        {
            GetPlayer.AddComponent(new Components.TestComponent(GetPlayer));
        }

        if (info.IsKeyPressed(Keys.NumPad0))
        {
            LookWindow w = new LookWindow(GetPlayer);
            w.Show();
        }

        if (info.IsKeyPressed(Keys.T))
        {
            foreach (NodeTile node in world.CurrentMap.Tiles.OfType<NodeTile>())
            {
                if (node.TrueAppearence.Matches(node))
                {
                    break;
                }
                node.RestoreOriginalAppearence();
            }
        }

        if (info.IsKeyPressed(Keys.Tab))
        {
            CommandManager.CreateNewMapForTesting();
        }
        if (info.IsKeyPressed(Keys.OemPlus))
        {
            try
            {
                // the map is being saved, but it isn't being properly deserialized
                Map map = (Map)GetPlayer.CurrentMap;
                map.LastPlayerPosition = GetPlayer.Position;
                if (GameLoop.World.MapIsWorld(map))
                {
                    string json = JsonConvert.SerializeObject(GameLoop.World.WorldMap);
                }
                else
                {
                    string json = map.SaveMapToJson(GetPlayer);
                    // The World class also isn't being serialized properly, crashing newtonsoft
                    // TODO: Revise this line of code when the time comes to work on the save system.
                    //var gameState = JsonConvert.SerializeObject(new GameState().World);
                    MapTemplate mapDeJsonified = JsonConvert.DeserializeObject<Map>(json);
                }
            }
            catch (Newtonsoft.Json.JsonSerializationException e)
            {
                throw e;
            }
        }

#endif

        return false;
    }

    private static bool CurrentMapIsPlanetView(World world)
    {
        if (world.WorldMap != null
            && world.WorldMap.AssocietatedMap == world.CurrentMap && world.Player == null)
            return true;
        else
            return false;
    }*/
    }
}
