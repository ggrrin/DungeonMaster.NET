using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class CreatureActuator : FloorActuator
    {

        public CreatureActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionStateX action) : base(position, currentTile, targetTile, action)
        {}

        protected override void TestAndRun(object enterginObject, bool objectEntered)
        {
            var creature = enterginObject as Creature;

            if (creature != null)
                Activate();
        }

        protected virtual void Activate()
        {
            AffectTile();
        }
    }
}
