using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall.FloorSensors
{
    class FloorSensorC02 : FloorSensor
    {
        protected override bool X(ref bool triggerSensor, object obj, bool entering, bool containsObj, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            if ((obj is IGrabableItem) || containsObj || alreadyContainsCreature)
            {
                return false;
            }
            return true;
        }

        public FloorSensorC02(SensorInitializer<IActuatorX> initializer) : base(initializer) {}
    }
}