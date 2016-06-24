using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class StorageSensorInitializer<TActuatorX>  : ItemConstrainSensorInitalizer<TActuatorX>  where TActuatorX : IActuatorX
    {
        public IGrabableItem StoredItem { get; set; }
    }
}