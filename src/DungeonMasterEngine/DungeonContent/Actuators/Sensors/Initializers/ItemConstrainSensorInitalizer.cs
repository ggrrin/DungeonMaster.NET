using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers
{
    public class ItemConstrainSensorInitalizer<TActuatorX> : SensorInitializer<TActuatorX> where TActuatorX : IActuatorX
    {
        public IGrabableItemFactoryBase Data { get; set; }
    }
}