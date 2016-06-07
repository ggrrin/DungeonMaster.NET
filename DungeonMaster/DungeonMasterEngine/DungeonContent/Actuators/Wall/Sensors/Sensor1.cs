using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    class Sensor1 : Sensor
    {
        protected override bool Interact(ILeader theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;
            if (Effect == SensorEffect.C03_EFFECT_HOLD)
            {
                return false;
            }
            return true;
        }

        public Sensor1(SensorInitializer initializer) : base(initializer) { }
    }
}