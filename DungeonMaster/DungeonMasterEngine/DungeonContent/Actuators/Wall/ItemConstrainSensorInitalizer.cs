using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class ItemConstrainSensorInitalizer<TActuatorX> : SensorInitializer<TActuatorX> where TActuatorX : IActuatorX
    {
        public IGrabableItemFactoryBase Data { get; set; }
    }
}