using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    abstract class SubXc017c011<TActuatorX> : ItemConstrainSensor<TActuatorX> where TActuatorX : IActuatorX
    {
        //process actutator
        protected override bool TryInteract(ILeader theron, WallActuator wallActuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (!isLast)
                return false;

            L0753_B_DoNotTriggerSensor = ((Data == theron.Hand?.Factory) == RevertEffect);

            if (!L0753_B_DoNotTriggerSensor)
                theron.Hand = null;

            return true;
        }

        public SubXc017c011(ItemConstrainSensorInitalizer<TActuatorX> initializer) : base(initializer) { }
    }
}