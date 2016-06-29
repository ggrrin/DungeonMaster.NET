using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.FloorSensors
{
    public abstract class FloorItemDataSensor : FloorSensor
    {
        public IGrabableItemFactoryBase Data { get; }

        public FloorItemDataSensor(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer)
        {
            Data = initializer.Data;
        }
    }
}