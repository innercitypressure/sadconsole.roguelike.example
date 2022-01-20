using System.Diagnostics;
using GoRogue;
using GoRogue.GameFramework;
using Roguelike.UI.Actors;
using Roguelike.UI.Entities;
using Roguelike.UI.Infrastructure.Tiles;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace Roguelike.UI.Infrastructure;

public class Map : GoRogue.GameFramework.Map
{
    private BaseTile[] tiles; // Contains all tiles objects
    private Entity gameObjectControlled;
    private readonly SadConsole.Entities.Renderer entityRender;
    public BaseTile[] Tiles { get { return tiles; } set { tiles = value; } }
    public event EventHandler FOVRecalculated;
    public event EventHandler<ControlledGameObjectChangedArgs> ControlledGameObjectChanged;
    public Entity ControlledEntitiy
    {
        get => gameObjectControlled;

        set
        {
            if (gameObjectControlled != value)
            {
                Entity oldObject = gameObjectControlled;
                gameObjectControlled = value;
                ControlledGameObjectChanged?.Invoke(this, new ControlledGameObjectChangedArgs(oldObject));
            }
        }
    }
    
    public string MapName { get; }
    
    public Map(string mapName, int width = 50, int height = 50) :
        base(CreateTerrain(width, height), Enum.GetNames(typeof(MapLayer)).Length - 1,
            Distance.Euclidean,
            entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask
                ((int)MapLayer.ITEMS, (int)MapLayer.PLAYER))
    {
        Tiles = (ArrayView<BaseTile>)((LambdaSettableTranslationGridView<BaseTile, IGameObject>)Terrain).BaseGrid;

       //  CalculateFOV(new Coord(gameObjectControlled.Position.X, gameObjectControlled.Position.Y), 20);
        
        entityRender = new SadConsole.Entities.Renderer();
        MapName = mapName;
    }
    
     #region HelperMethods

        private static ISettableGridView<IGameObject> CreateTerrain(int width, int height)
        {
            var goRogueTerrain = new ArrayView<BaseTile>(width, height);
            return new LambdaSettableTranslationGridView<BaseTile, IGameObject>(goRogueTerrain, t => t, g => g as BaseTile);
        }

        /// <summary>
        /// IsTileWalkable checks
        /// to see if the actor has tried
        /// to walk off the map or into a non-walkable tile
        /// Returns true if the tile location is walkable
        /// false if tile location is not walkable or is off-map
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool IsTileWalkable(Point location)
        {
            // first make sure that actor isn't trying to move
            // off the limits of the map
            if (CheckForIndexOutOfBounds(location))
                return false;

            // then return whether the tile is walkable
            return !tiles[location.Y * Width + location.X].IsBlockingMove;
        }

        /// <summary>
        /// IsTileWalkable checks
        /// to see if the actor has tried
        /// to walk off the map or into a non-walkable tile
        /// Returns true if the tile location is walkable
        /// false if tile location is not walkable or is off-map
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool IsTileWalkable(Point location, Actor actor)
        {
            // first make sure that actor isn't trying to move
            // off the limits of the map
            if (CheckForIndexOutOfBounds(location))
                return false;

            // then return whether the tile is walkable
            return !tiles[location.Y * Width + location.X].IsBlockingMove;
        }

        /// <summary>
        /// Checking whether a certain type of
        /// entity is at a specified location the manager's list of entities
        /// and if it exists, return that Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <returns></returns>
        public T GetEntityAt<T>(Point location) where T : Entity
        {
            return Entities.GetItemsAt(location).OfType<T>().FirstOrDefault(e => e.CanInteract);
        }

        public Entity GetClosestEntity(Point originPos, int range)
        {
            Entity closest = null;
            double bestDistance = double.MaxValue;

            foreach (Entity entity in Entities.Items)
            {
                if (entity is not Player)
                {
                    double distance = Point.EuclideanDistanceMagnitude(new Point(originPos.X, originPos.Y), new Point(entity.Position.X, entity.Position.Y));

                    if (distance < bestDistance && (distance <= range || range == 0))
                    {
                        bestDistance = distance;
                        closest = entity;
                    }
                }
            }

            return closest;
        }

        /// <summary>
        /// Removes an Entity from the Entities Field
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(Entity entity)
        {
            RemoveEntity(entity);

            entityRender.Remove(entity);

            // Link up the entity's Moved event to a new handler
            entity.Moved -= OnEntityMoved;
        }

        /// <summary>
        /// Adds an Entity to the Entities field
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Entity entity)
        {
            if (entity.CurrentMap is not null)
            {
                Map map = (Map)entity.CurrentMap;
                map.ControlledEntitiy = null;
                map.Remove(entity);
            }
            // Initilizes the field of view of the player, will do different for monsters
            if (entity is Player player)
            {
                FovCalculate(player);
                ControlledEntitiy = player;
            }

            try
            {
                AddEntity(entity);
            }
            catch (ArgumentException)
            {
                entity.Position = GetRandomWalkableTile();
                AddEntity(entity);
#if DEBUG
                Debug.Print("An entity tried to telefrag another");
#endif
            }

            entityRender.Add(entity);

            // Link up the entity's Moved event to a new handler
            entity.Moved += OnEntityMoved;
        }

        /// <summary>
        /// When the Entity's .Moved value changes, it triggers this event handler
        /// which updates the Entity's current position in the SpatialMap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnEntityMoved(object sender, GameObjectPropertyChanged<Point> args)
        {
            if (args.Item is Player player)
            {
                FovCalculate(player);
            }

            entityRender.IsDirty = true;
        }

        /// <summary>
        /// really snazzy way of checking whether a certain type of
        /// tile is at a specified location in the map's Tiles
        /// and if it exists, return that Tile
        /// accepts an x/y coordinate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T GetTileAt<T>(int x, int y) where T : BaseTile
        {
            int locationIndex = Point.ToIndex(x, y, Width);

            // make sure the index is within the boundaries of the map!
            if (locationIndex <= Width * Height && locationIndex >= 0)
            {
                return Tiles[locationIndex] is T t ? t : null;
            }
            else return null;
        }

        /// <summary>
        /// Checks if a specific type of tile at a specified location
        /// is on the map. If it exists, returns that Tile
        /// This form of the method accepts a Point coordinate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <returns></returns>
        public T GetTileAt<T>(Point location) where T : BaseTile
        {
            return GetTileAt<T>(location.X, location.Y);
        }

        public void RemoveAllTiles()
        {
            foreach (BaseTile tile in Tiles)
            {
                RemoveTerrain(tile);
            }
        }

        /// <summary>
        /// Gets the entity by it's id
        /// </summary>
        /// <typeparam name="T">Any type of entity</typeparam>
        /// <param name="id">The id of the entity to find</param>
        /// <returns>Returns the entity owner of the id</returns>
        public Entity GetEntityById(uint id)
        {
            // TODO: this shit is wonky, need to do something about it
            var filter = from entity in Entities.Items
                         where entity.ID == id
                         select entity;

            if (filter.Any())
            {
                return (Entity)filter.FirstOrDefault();
            }
            return null;
        }

        public void ConfigureRender(SadConsole.Console renderer)
        {
            renderer.SadComponents.Add(entityRender);
            entityRender.DoEntityUpdate = true;

            foreach (Entity item in Entities.Items)
            {
                entityRender.Add(item);
            }
            renderer.IsDirty = true;
        }

        private void FovCalculate(Actor actor)
        {
            CalculateFOV(new Coord(actor.Position.X, actor.Position.Y), 20);
            FOVRecalculated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fix for the FOV problem that sometimes don't work
        /// </summary>
        public void ForceFovCalculation()
        {
            FovCalculate((Actor)ControlledEntitiy);
        }

        /// <summary>
        /// This is used to get a random point that is walkable inside the map, mainly used when adding an entity
        /// and there is already an entity there, so it picks another random location.
        /// </summary>
        /// <returns>Returns an Point to a tile that is walkable and there is no actor there</returns>
        public Point GetRandomWalkableTile()
        {
            var rng = GoRogue.Random.SingletonRandom.DefaultRNG;
            Point rngPoint = new Point(rng.Next(Width - 1), rng.Next(Height - 1));

            while (!IsTileWalkable(rngPoint))
                rngPoint = new Point(rng.Next(Width - 1), rng.Next(Height - 1));

            return rngPoint;
        }

        /// <summary>
        /// Returns if the Point is inside the index of the map, makes sure that nothing tries to go outside the map.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool CheckForIndexOutOfBounds(Point point)
        {
            if (point.X < 0 || point.Y < 0
                || point.X >= Width || point.Y >= Height)
            {
                return true;
            }

            return false;
        }

        #endregion HelperMethods

    
    public enum MapLayer
    {
        TERRAIN,
        ITEMS,
        ACTORS,
        FURNITURE,
        PLAYER
    }
    
    public class ControlledGameObjectChangedArgs : EventArgs
    {
        public Entity OldObject;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="oldObject"/>
        public ControlledGameObjectChangedArgs(Entity oldObject) => OldObject = oldObject;
    }
}