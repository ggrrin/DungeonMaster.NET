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
    public class TheronPartyCreatureItemActuator : FloorActuator
    {
        public TheronPartyCreatureItemActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionStateX action) : base(position, currentTile, targetTile, action)
        {}

        
        protected override void TestAndRun(object e, bool objectEnter)
        {
            bool activated = e is Theron || e is Creature || e is GrabableItem;//TODO party test not only Theron 1!!!!

            if (activated)
                Activate();
        }

        protected virtual void Activate()
        {
            AffectTile();
        }


    }
}
