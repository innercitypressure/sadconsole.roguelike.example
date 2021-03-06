using Roguelike.UI.Infrastructure;
using SadConsole.Entities;
using SadRogue.Primitives;

namespace Roguelike.UI.Entities;

public class Actor : Entity
{
        #region Fields

        private bool bumped = false;
        // private Stat stats = new Stat();

        #endregion Fields

        #region Properties

        /// <summary>
        /// The stats of the actor
        /// </summary>
        // public Stat Stats { get => stats; set => stats = value; }

        /// <summary>
        /// The anatomy of the actor
        /// </summary>
        // public Anatomy Anatomy { get; set; }

        /// <summary>
        /// Sets if the char has bumbed in something
        /// </summary>
        public bool Bumped { get => bumped; set => bumped = value; }

        /// <summary>
        /// Here we define were the inventory is
        /// </summary>
       //  public List<Item> Inventory { get; set; }

        /// <summary>
        /// The equipment that the actor is curently using
        /// </summary>
        // [JsonIgnore]
        // public Dictionary<Limb, Item> Equipment { get; set; }

        // [JsonIgnore]
        // public int XP { get; set; }

        /// <summary>
        /// Dictionary of the Abilities of an actor.
        /// Never add directly to the dictionary, use the method AddAbilityToDictionary to add new abilities
        /// </summary>
        // public Dictionary<int, Ability> Abilities { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// This here defines an actor, must be used with the <see cref= "Data.EntityFactory">
        /// Normally, use the ActorCreator method!
        /// </summary>
        /// <param name="foreground"></param>
        /// <param name="background"></param>
        /// <param name="glyph"></param>
        /// <param name="layer"></param>
        /// <param name="coord"></param>
        public Actor(string name, Color foreground, Color background,
            int glyph, Point coord, int layer = (int)MapLayer.ACTORS
            ) : base(foreground, background,
            glyph, coord, layer)
        {
            // Anatomy = new Anatomy(this);
            // Inventory = new List<Item>();
            // Equipment = new Dictionary<Limb, Item>();
            // Abilities = new();
            Name = name;
            // by default the material of the actor will be mostly flesh
            // Material = System.Physics.PhysicsManager.SetMaterial("flesh");
        }

        #endregion Constructor

        #region HelpCode

        // Moves the Actor BY positionChange tiles in any X/Y direction
        // returns true if actor was able to move, false if failed to move
        // Checks for Monsters, and allows the Actor to commit
        // an action if one is present.
        // TODO: an autopickup feature for items
        public bool MoveBy(Point positionChange)
        {
            // Check the current map if we can move to this new position
            if (Program.World.CurrentMap.IsTileWalkable(Position + positionChange, this))
            {
                bool attacked = CheckIfCanAttack(positionChange);

                if (attacked)
                    return attacked;

                Position += positionChange;

                return true;
            }

            // Handle situations where there are non-walkable tiles that CAN be used
            else
            {
                bool doorThere = CheckIfThereIsDoor(positionChange);

                if (doorThere)
                    return doorThere;

                return false;
            }
        }

        private bool CheckIfCanAttack(Point positionChange)
        {
            // if there's a monster here,
            // do a bump attack
            Actor actor = Program.World.CurrentMap.GetEntityAt<Actor>(Position + positionChange);

            if (actor != null && CanBeAttacked)
            {
                // TODO: Uncomment to attack!
                // CommandManager.Attack(this, actor);
                Bumped = true;
                return Bumped;
            }

            Bumped = false;
            return Bumped;
        }

        private bool CheckIfThereIsDoor(Point positionChange)
        {
            // Check for the presence of a door
            // Door door = GameLoop.Universe.CurrentMap.GetTileAt<TileDoor>(Position + positionChange);

            // if there's a door here,
            // try to use it
            /*if (door != null && CanInteract)
            {
                CommandManager.UseDoor(this, door);
                GameLoop.UIManager.MapWindow.MapConsole.IsDirty = true;
                return true;
            }*/

            return false;
        }

        // Moves the Actor TO newPosition location
        // returns true if actor was able to move, false if failed to move
        public bool MoveTo(Point newPosition)
        {
            if (Program.World.CurrentMap.IsTileWalkable(newPosition))
            {
                Position = newPosition;
                return true;
            }
            else
            {
                return false;
            }
        }

        /*public Item WieldedItem()
        {
            return Equipment.GetValueOrDefault(Anatomy.Limbs.Find(l => l.TypeLimb == TypeOfLimb.Hand));
        }*/

        /*public void AddAbilityToDictionary(Ability ability)
        {
            Abilities.Add(ability.Id, ability);
        }*/

        #endregion HelpCode
}