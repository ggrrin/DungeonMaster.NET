using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    class Sensor2 : Sensor<IActuatorX>
    {
        protected override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = ((theron.Hand == null) != RevertEffect);
            return true;
        }

        public Sensor2(SensorInitializer<IActuatorX> initializer) : base(initializer) { }
    }
}