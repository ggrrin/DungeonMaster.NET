using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.FloorSensors
{
    class FloorSensorC04 : FloorItemDataSensor 
    {
        protected override bool TryInteract(ref bool triggerSensor, object obj, bool entering, bool containsObj, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            var item = obj as IGrabableItem;

            if (item == null || (Data != item.FactoryBase) || containsThingOfSameType)
            {
                return false;
            }
            return true;
        }

        public FloorSensorC04(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer) {}
    }
}