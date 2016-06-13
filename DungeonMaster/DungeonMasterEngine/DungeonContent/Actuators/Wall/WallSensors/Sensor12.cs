using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    class Sensor12 : ItemConstrainSensor<IActuatorX>
    {
        protected override bool Interact(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (!isLast)
                return false;

            L0753_B_DoNotTriggerSensor = theron.Hand != null;
            if (!L0753_B_DoNotTriggerSensor)
            {
                F270_xxxx_SENSOR_TriggerLocalEffect(theron, actuator, true); /* This will cause a rotation of the sensors at the specified cell on the specified square after all sensors have been processed */
                theron.Hand = Data.Create();
            }
            return true;
        }

        public Sensor12(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer) { }
    }
}