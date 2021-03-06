namespace Roguelike.UI.Infrastructure.Time;

public class EntityTimeNode : ITimeNode
{
    public EntityTimeNode(uint entityId, long tick)
    {
        Tick = tick;
        EntityId = entityId;
    }

    public long Tick { get; }

    public uint EntityId { get; init; }
}