using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    class Sensor17 : SubXc017c011<IActuatorX>
    {
        protected override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            if (!base.TryInteract(theron, actuator, isLast, out L0753_B_DoNotTriggerSensor))
                return false;

            if (!L0753_B_DoNotTriggerSensor)
            {
                if (actuator.Sensors.Count == 1)
                    return true;

                actuator.Sensors.Remove(this);
            }
            return true;
        }

        public Sensor17(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer) { }
    }
}