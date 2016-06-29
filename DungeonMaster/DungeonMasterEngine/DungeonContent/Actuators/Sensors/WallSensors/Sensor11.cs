using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    class Sensor11 : SubXc017c011<IActuatorX>
    {
        protected override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            if (!base.TryInteract(theron, actuator, isLast, out L0753_B_DoNotTriggerSensor))
                return false; ;

            if (!L0753_B_DoNotTriggerSensor)
            {
                TriggerLocalEffect(theron, actuator, true); /* This will cause a rotation of the sensors at the specified cell on the specified square after all sensors have been processed */
            }
            return true;
        }

        public Sensor11(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer) { }
    }
}