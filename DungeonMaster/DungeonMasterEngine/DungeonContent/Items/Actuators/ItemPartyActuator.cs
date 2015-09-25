using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using DungeonMasterEngine.Items;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class ItemPartyActuator : PartyActuator
    {
        public IConstrain Constrain { get; }

     

        public ItemPartyActuator(Vector3 position, Tile actuatorTile, Tile targetTile, IConstrain constrain) : base(position, actuatorTile, targetTile)
        {
            Constrain = constrain;
        }

        protected override void Activate(Theron theron)
        {
            if (null != (from i in new GrabableItem[] { theron.Hand }.Concat(theron.PartyGroup.SelectMany(x => x.Inventory)) where Constrain.IsAcceptable(i) select i).FirstOrDefault())
                TargetTile.ActivateTileContent();
        }
    }
}
