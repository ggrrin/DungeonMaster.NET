using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class TheronPartyCreatureActuator : FloorActuator
    {
        public TheronPartyCreatureActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionStateX action) : base(position, currentTile, targetTile, action)
        {
        }

        protected override void TestAndRun(object enteringObject, bool objectEntered)
        {
            bool activated = enteringObject is Theron || enteringObject is Creature;

            if (activated)
                Activate();
        }
        protected virtual void Activate()
        {
            AffectTile();
        }
    }
}
