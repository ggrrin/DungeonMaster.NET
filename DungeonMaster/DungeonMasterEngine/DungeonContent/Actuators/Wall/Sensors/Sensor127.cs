using System;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class Sensor127 : Sensor
    {
        public override bool TryTrigger(ILeader theron, ActuatorX actuator, bool isLast)
        {
            if (theron.Leader == null)
                return false;

            return base.TryTrigger(theron, actuator, isLast);
        }

        protected override bool Interact(ILeader theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            //TODO add add
            throw new NotImplementedException();
            return false;
        }

        public Sensor127(SensorInitializer initializer) : base(initializer) { }
    }
}