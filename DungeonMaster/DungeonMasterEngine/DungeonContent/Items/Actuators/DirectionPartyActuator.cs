using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class DirectionPartyActuator : PartyActuator
    {
        public DirectionPartyActuator(Vector3 position, Tile tile, Tile remoteTile) : base(position, tile, remoteTile)
        {}

        protected override void Activate(Theron theron)
        {
            //TODO direction constrain
            TargetTile.ActivateTileContent();
        }
    }
}
