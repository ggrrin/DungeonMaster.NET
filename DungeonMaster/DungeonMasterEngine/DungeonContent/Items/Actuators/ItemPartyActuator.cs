using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class ItemPartyActuator : PartyActuator
    {
        public IConstrain Constrain { get; }

        public ItemPartyActuator(Vector3 position, Tile actuatorTile, Tile targetTile, IConstrain constrain, ActionStateX action) : base(position, actuatorTile, targetTile, action)
        {
            Constrain = constrain;
        }

        protected override void Activate(Theron theron)
        {
            if (null != (from i in new GrabableItem[] { theron.Hand }.Concat(theron.PartyGroup.SelectMany(x => x.Inventory)) where Constrain.IsAcceptable(i) select i).FirstOrDefault())
                AffectTile();
        }
    }
}
