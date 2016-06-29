using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.FloorSensors
{
    class FloorSensorC08 : FloorItemDataSensor
    {
        protected override bool TryInteract(ref bool triggerSensor, object obj, bool entering, bool containsObj, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
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