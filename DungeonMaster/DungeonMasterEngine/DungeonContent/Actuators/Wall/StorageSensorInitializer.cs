using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class StorageSensorInitializer<TActuatorX>  : ItemConstrainSensorInitalizer<TActuatorX>  where TActuatorX : IActuatorX
    {
        public IGrabableItem StoredItem { get; set; }
    }
}