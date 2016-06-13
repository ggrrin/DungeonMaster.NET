using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    class Sensor13 : ItemConstrainSensor<IActuatorX>
    {
        public IGrabableItem Storage { get; private set; }

        protected override bool Interact(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (theron.Hand == null)
            {
                if ((theron.Hand = Storage) == null)
                    return false;

                Storage = null;
            }
            else
            {
                if ((theron.Hand.Factory != Data) || Storage != null)
                    return false;

                Storage = theron.Hand;
                theron.Hand = null;
            }

            F270_xxxx_SENSOR_TriggerLocalEffect(theron, actuator, true); /* This will cause a rotation of the sensors at the specified cell on the specified square after all sensors have been processed */
            if ((Effect == SensorEffect.C03_EFFECT_HOLD) && theron.Hand != null)
            {
                L0753_B_DoNotTriggerSensor = true;
            }
            else
            {
                L0753_B_DoNotTriggerSensor = false;
            }
            return true;
        }

        public Sensor13(StorageSensorInitializer<IActuatorX> initializer) : base(initializer)
        {
            Storage = initializer.StoredItem;
        }
    }
}