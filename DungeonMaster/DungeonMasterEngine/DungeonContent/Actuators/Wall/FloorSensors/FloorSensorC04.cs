using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall.FloorSensors
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

    public abstract class FloorItemData : FloorSensor
    {
        public IGrabableItemFactoryBase Data { get; }

        public FloorItemData(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer)
        {
            Data = initializer.Data;
        }
    }
}