using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    class Sensor12 : ItemConstrainSensor<IActuatorX>
    {
        protected override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (!isLast)
                return false;

            L0753_B_DoNotTriggerSensor = theron.Hand != null;
            if (!L0753_B_DoNotTriggerSensor)
            {
                TriggerLocalEffect(theron, actuator, true); /* This will cause a rotation of the sensors at the specified cell on the specified square after all sensors have been processed */
                theron.Hand = Data.CreateItem();
            }
            return true;
        }

        public Sensor12(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer) { }
    }
}