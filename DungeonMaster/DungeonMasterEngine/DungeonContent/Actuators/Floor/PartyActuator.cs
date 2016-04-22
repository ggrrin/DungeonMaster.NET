using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public abstract class PartyActuator : FloorActuator
    {
        public PartyActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionStateX action) : base(position, currentTile, targetTile, action)
        { }

        protected override void TestAndRun(object enteringObect, bool objectEnter)
        {
            var theron = (enteringObect as Theron);

            if (Enabled && theron?.PartyGroup.Count == 4)
            {
                Activate(theron);
            }
        }

        protected abstract void Activate(Theron theron);
    }
}
