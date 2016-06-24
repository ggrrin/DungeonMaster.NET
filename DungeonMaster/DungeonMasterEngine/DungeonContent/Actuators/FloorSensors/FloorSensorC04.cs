using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Actuators.FloorSensors
{
    class FloorSensorC04 : FloorItemData 
    {
        protected override bool X(ref bool triggerSensor, object obj, bool entering, bool containsObj, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            var item = obj as IGrabableItem;

            if (item == null || (Data != item.Factory) || containsThingOfSameType)
            {
                return false;
            }
            return true;
        }

        public FloorSensorC04(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer) {}
    }
}