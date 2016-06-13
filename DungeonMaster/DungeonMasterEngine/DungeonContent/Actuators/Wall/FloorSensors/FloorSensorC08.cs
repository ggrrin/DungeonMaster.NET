using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall.FloorSensors
{
    class FloorSensorC08 : FloorItemData
    {
        protected override bool X(ref bool triggerSensor, object obj, bool entering, bool containsObj, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            var theron = obj as ILeader;
            if (theron == null)
            {
                return false;
            }
            triggerSensor = theron.PartyGroup.
                SelectMany(x => x.Body.Storages.SelectMany(s => s.Storage.Where(y => y != null)))
                .Concat(new[] { theron.Hand }.Where(x => x != null))
                .Any(x => x.Factory == Data);
            return true;
        }

        public FloorSensorC08(ItemConstrainSensorInitalizer<IActuatorX> initializer) : base(initializer) { }
    }
}