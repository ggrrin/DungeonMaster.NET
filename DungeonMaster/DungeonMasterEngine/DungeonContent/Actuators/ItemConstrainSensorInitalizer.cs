using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class ItemConstrainSensorInitalizer<TActuatorX> : SensorInitializer<TActuatorX> where TActuatorX : IActuatorX
    {
        public IGrabableItemFactoryBase Data { get; set; }
    }
}