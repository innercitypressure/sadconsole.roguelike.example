using GoRogue;

namespace Roguelike.UI.Infrastucture;

public class Map : GoRogue.GameFramework.Map
{
    public Map(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement, uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295, uint entityLayersSupportingMultipleItems = 0) : base(width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems)
    {
    }
}