using DungeonMasterEngine.DungeonContent.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers
{
    public class StorageSensorInitializer<TActuatorX>  : ItemConstrainSensorInitalizer<TActuatorX>  where TActuatorX : IActuatorX
    {
        public IGrabableItem StoredItem { get; set; }
    }
}