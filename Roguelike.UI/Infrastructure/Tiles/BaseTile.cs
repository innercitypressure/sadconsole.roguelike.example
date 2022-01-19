using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using SadRogue.Primitives;
using Direction = GoRogue.Direction;

namespace Roguelike.UI.Infrastructure.Tiles;

public class BaseTile : ColoredGlyph, IGameObject
{
    private readonly IGameObject backingField;
    public bool IsBlockingMove { get; set; }
    public int Layer { get; set; }
    public ColoredGlyph LastSeenAppearance { get; set; }
    
    public string Name { get; set; }
    
    protected BaseTile(Color foregroud, Color background, int glyph, int layer,
        Coord position, string idOfMaterial, bool blocksMove = true,
        bool isTransparent = true, string name = "ForgotToChangeName") : base(foregroud, background, glyph)
    {
        IsBlockingMove = blocksMove;
        Name = name;
        Layer = layer;
        backingField = new GameObject(position, layer, this, isTransparent);
        LastSeenAppearance = new ColoredGlyph(Foreground, Background, Glyph)
        {
            IsVisible = false
        };
    }

    public uint ID { get; }
    public void AddComponent(object component)
    {
        throw new NotImplementedException();
    }

    public T GetComponent<T>()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetComponents<T>()
    {
        throw new NotImplementedException();
    }

    public bool HasComponent(Type componentType)
    {
        throw new NotImplementedException();
    }

    public bool HasComponent<T>()
    {
        throw new NotImplementedException();
    }

    public bool HasComponents(params Type[] componentTypes)
    {
        throw new NotImplementedException();
    }

    public void RemoveComponent(object component)
    {
        throw new NotImplementedException();
    }

    public void RemoveComponents(params object[] components)
    {
        throw new NotImplementedException();
    }

    public bool MoveIn(Direction direction)
    {
        throw new NotImplementedException();
    }

    public void OnMapChanged(GoRogue.GameFramework.Map newMap)
    {
        throw new NotImplementedException();
    }

    public GoRogue.GameFramework.Map CurrentMap { get; }
    public bool IsStatic { get; }
    public bool IsTransparent { get; set; }
    public bool IsWalkable { get; set; }
    public Coord Position { get; set; }
    public event EventHandler<ItemMovedEventArgs<IGameObject>>? Moved;
}
