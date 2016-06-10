using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class ItemConstrainSensorInitalizer : SensorInitializer
    {
        public IGrabableItemFactoryBase Data { get; set; }
    }
}