using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class TheronPartyCreatureActuator : FloorActuator
    {
        public TheronPartyCreatureActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionState action) : base(position, currentTile, targetTile, action)
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
