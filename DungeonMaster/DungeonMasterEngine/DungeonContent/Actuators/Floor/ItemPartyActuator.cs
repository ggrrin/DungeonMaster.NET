using System.Linq;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
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
