using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class PartDirectionConstrain : IConstrain
    {
        public MapDirection AcceptDirection { get; }

        public PartDirectionConstrain(MapDirection acceptDirection)
        {
            AcceptDirection = acceptDirection;
        }

        public bool IsAcceptable(object item)
        {
            var theron = item as Theron;
            return theron?.PartyGroup.Count == 4 && theron.MapDirection == AcceptDirection;
        }
    }
}
