using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.WallSensors
{
    class Sensor4 : ItemConstrainSensor<IActuatorX>
    {
        protected override bool Interact(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = ((Data == theron.Hand?.Factory) == RevertEffect);
            if (!L0753_B_DoNotTriggerSensor)
                theron.Hand = null;
            return true; ;
        }

        public Sensor4(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer) { }
    }
}