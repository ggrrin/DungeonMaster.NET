using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Actuators.FloorSensors
{
    public abstract class FloorItemData : FloorSensor
    {
        public IGrabableItemFactoryBase Data { get; }

        public FloorItemData(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer)
        {
            Data = initializer.Data;
        }
    }
}