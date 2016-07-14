using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    class Sensor16 : ItemConstrainWallSensor<IActuatorX>
    {
        public IGrabableItem Storage { get; private set; }

        protected override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (!isLast)
            { /* If the sensor is not the last one of its type on the cell */
                return false;
            }
            //F162_afzz_DUNGEON_GetSquareFirstObject(); //TODO is it possible to put item from one side of the wall and take it from antoher ? 
            if ((theron.Hand?.FactoryBase != Data) || (Storage == null))
                return false;

            var handItem = theron.Hand;
            theron.Hand = Storage;
            Storage = handItem;
            L0753_B_DoNotTriggerSensor = false;
            return true;
        }

        public Sensor16(StorageSensorInitializer<IActuatorX> initializer) : base(initializer)
        {
            Storage = initializer.StoredItem;
        }
    }
}