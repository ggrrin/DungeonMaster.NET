using System;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.WallSensors
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