using System;
using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    public class Sensor0 : WallSensor<IActuatorX>
    {

        public Sensor0(SensorInitializer<IActuatorX> initializer) : base(initializer)
        {

            Disabled = true;
        }

        protected override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            throw new NotImplementedException();
        }
    }
}