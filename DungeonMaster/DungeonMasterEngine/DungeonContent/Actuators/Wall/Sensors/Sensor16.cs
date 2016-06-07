using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    class Sensor16 : ItemConstrainSensor
    {
        public IGrabableItem Storage { get; private set; }

        protected override bool Interact(ILeader theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (!isLast)
            { /* If the sensor is not the last one of its type on the cell */
                return false;
            }
            //F162_afzz_DUNGEON_GetSquareFirstObject(); //TODO is it possible to put item from one side of the wall and take it from antoher ? 
            if ((theron.Hand.Factory != Data) || (Storage == null))
                return false;

            var handItem = theron.Hand;
            theron.Hand = Storage;
            Storage = handItem;
            L0753_B_DoNotTriggerSensor = false;
            return true;
        }

        public Sensor16(StorageSensorInitializer initializer) : base(initializer)
        {
            Storage = initializer.StoredItem;
        }
    }
}