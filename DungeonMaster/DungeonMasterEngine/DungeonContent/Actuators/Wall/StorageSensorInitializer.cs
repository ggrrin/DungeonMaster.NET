using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class StorageSensorInitializer : ItemConstrainSensorInitalizer 
    {
        public IGrabableItem StoredItem { get; set; }
    }
}