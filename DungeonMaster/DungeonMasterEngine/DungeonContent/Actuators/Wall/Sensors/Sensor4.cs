using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    class Sensor4 : ItemConstrainSensor
    {
        protected override bool Interact(ILeader theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = ((Data == theron.Hand.Factory) == RevertEffect);
            if (!L0753_B_DoNotTriggerSensor)
                theron.Hand = null;
            return true; ;
        }

        public Sensor4(ItemConstrainSensorInitalizer initializer) : base(initializer) { }
    }
}