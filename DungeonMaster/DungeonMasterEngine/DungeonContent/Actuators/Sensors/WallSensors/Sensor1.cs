using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    class Sensor1 : Sensor<IActuatorX>
    {
        protected override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;
            if (Effect == SensorEffect.C03_EFFECT_HOLD)
            {
                return false;
            }
            return true;
        }

        public Sensor1(SensorInitializer<IActuatorX> initializer) : base(initializer) { }
    }
}