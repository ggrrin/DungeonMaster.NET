using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.FloorSensors
{
    class FloorSensorC03 : FloorSensor
    {
        public MapDirection? Direction { get; }

        protected override bool TryInteract(ref bool triggerSensor, object obj, bool entering, bool containsObj, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            if (!(obj is ILeader) || ((ILeader) obj).PartyGroup.Count == 0)
            {
                return false;
            }

            if (Direction == null)
            {
                if (containsObj)
                {
                    return false;
                }
            }
            else
            {
                if (!entering)
                {
                    triggerSensor = false;
                }
                else
                {
                    triggerSensor = Direction.Value == ((ILeader) obj).MapDirection;
                }
            }
            return true;
        }

        public FloorSensorC03(DirectionIntializer initializer) : base(initializer)
        {
            Direction = initializer.Direction;
        }
    }
}