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
    public class TheronPartyCreatureItemActuator : FloorActuator
    {
        public TheronPartyCreatureItemActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionState action) : base(position, currentTile, targetTile, action)
        {}

        
        protected override void TestAndRun(object e, bool objectEnter)
        {
            bool activated = e is Theron || e is Creature || e is GrabableItem;

            if (activated)
                Activate();
        }

        protected virtual void Activate()
        {
            AffectTile();
        }


    }
}
