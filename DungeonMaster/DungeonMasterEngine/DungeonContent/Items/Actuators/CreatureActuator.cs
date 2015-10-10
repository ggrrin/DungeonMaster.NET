using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class CreatureActuator : FloorActuator
    {

        public CreatureActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionState action) : base(position, currentTile, targetTile, action)
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
