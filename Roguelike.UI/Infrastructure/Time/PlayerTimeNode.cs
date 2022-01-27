namespace Roguelike.UI.Infrastructure.Time;

public class PlayerTimeNode : ITimeNode
{
    public PlayerTimeNode(long tick)
    {
        Tick = tick;
    }

    public long Tick { get; }
}