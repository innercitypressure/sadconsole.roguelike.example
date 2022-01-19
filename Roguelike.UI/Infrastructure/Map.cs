using GoRogue;

namespace Roguelike.UI.Infrastructure;

public class Map : GoRogue.GameFramework.Map
{
    
    // TODO : Needs to be changed
    public Map(string name, int width, int height, int numberOfEntityLayers, Distance distanceMeasurement, uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295, uint entityLayersSupportingMultipleItems = 0) : base(width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems)
    {
    }
    
    public enum MapLayer
    {
        TERRAIN,
        ITEMS,
        ACTORS,
        FURNITURE,
        PLAYER
    }
}