using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.WallSensors
{
    class Sensor3 : ItemConstrainSensor<IActuatorX>
    {
        protected override bool Interact(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = ((Data == theron.Hand?.Factory) == RevertEffect);
            return true; ;
        }

        public Sensor3(ItemConstrainSensorInitalizer<IActuatorX>  initializer) : base(initializer) { }
    }
}