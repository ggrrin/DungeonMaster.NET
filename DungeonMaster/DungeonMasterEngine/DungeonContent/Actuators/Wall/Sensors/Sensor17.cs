using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    class Sensor17 : SubXc017c011
    {
        protected override bool Interact(ILeader theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            if (!base.Interact(theron, actuator, isLast, out L0753_B_DoNotTriggerSensor))
                return false;

            if (!L0753_B_DoNotTriggerSensor)
            {
                if (actuator.Sensors.Count == 1)
                    return true;

                actuator.Sensors.Remove(this);
            }
            return true;
        }

        public Sensor17(ItemConstrainSensorInitalizer initializer) : base(initializer) { }
    }
}