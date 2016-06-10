using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    class Sensor3 : ItemConstrainSensor
    {
        protected override bool Interact(ILeader theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = ((Data == theron.Hand.Factory) == RevertEffect);
            return true; ;
        }

        public Sensor3(ItemConstrainSensorInitalizer  initializer) : base(initializer) { }
    }
}