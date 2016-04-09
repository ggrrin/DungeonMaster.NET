using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Player;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
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
