using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;

namespace Roguelike.UI.Interface.Windows;

public class BaseMap : GoRogue.GameFramework.Map
{
    public BaseMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement, uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295, uint entityLayersSupportingMultipleItems = 0) : base(width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems)
    {
    }

    public BaseMap(ISettableMapView<IGameObject> terrainLayer, int numberOfEntityLayers, Distance distanceMeasurement, uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295, uint entityLayersSupportingMultipleItems = 0) : base(terrainLayer, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems)
    {
    }
}