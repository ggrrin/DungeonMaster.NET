using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.FloorSensors
{
    class FloorSensorC07 : FloorSensor
    {
        protected override bool X(ref bool triggerSensor, object obj, bool entering, bool containsObj, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            if ((obj is IGrabableItem) || (obj is ILeader) || alreadyContainsCreature)
            {
                return false;
            }
            return true;
        }

        public FloorSensorC07(SensorInitializer<IActuatorX> initializer) : base(initializer) {}
    }
}