using System;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class Sensor0 : Sensor<IActuatorX>
    {

        public Sensor0(SensorInitializer<IActuatorX> initializer) : base(initializer)
        {

            Disabled = true;
        }

        protected override bool Interact(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            throw new NotImplementedException();
        }
    }
}