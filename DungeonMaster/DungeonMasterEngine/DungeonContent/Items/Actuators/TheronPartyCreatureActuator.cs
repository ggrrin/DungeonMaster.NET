using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
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
