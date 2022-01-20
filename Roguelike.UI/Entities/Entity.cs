using GoRogue.GameFramework;
using SadConsole;
using SadRogue.Primitives;
using GoRogue;
using Direction = GoRogue.Direction;

namespace Roguelike.UI.Entities;

public class Entity : SadConsole.Entities.Entity, IGameObject
{
    public uint ID { get; }
    public int Layer { get; set; }
    public Map CurrentMap { get; }
    public bool IsStatic { get; }
    public bool IsTransparent { get; set; }
    public bool IsWalkable { get; set; }
    public Coord Position { get; set; }
    public IGameObject BackingField { get; set; }
    
    public event EventHandler<ItemMovedEventArgs<IGameObject>>? Moved;
    
    private void Position_Changed(object? sender, ValueChangedEventArgs<Point> e)
        => Moved?.Invoke(sender, new ItemMovedEventArgs<IGameObject>(this, new Coord(e.OldValue.X, e.OldValue.Y), new Coord(e.NewValue.X, e.NewValue.Y)));
    
    public Entity(Color foreground, Color background, int glyph, int zIndex, Point coord) : base(foreground, background, glyph, zIndex)
    {
        InitializeObject(foreground, background, glyph, zIndex, coord);
    }
    
    private void InitializeObject(
        Color foreground, Color background, int glyph, int layer, Point coord)
    {
        Appearance.Foreground = foreground;
        Appearance.Background = background;
        Appearance.Glyph = glyph;
        Layer = layer;

        BackingField = new GameObject(new Coord(coord.X, coord.Y), layer, this);
        Position = BackingField.Position;

        PositionChanged += Position_Changed;
    }
    
    public void AddComponent(object component)
    {
        BackingField.AddComponent(component);
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

    public void OnMapChanged(Map newMap)
    {
        BackingField.OnMapChanged(newMap);
    }
}