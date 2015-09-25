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
    public class TheronPartyCreatureActuator : Actuator
    {
        public Tile TargetTile { get; }

        public TheronPartyCreatureActuator(Vector3 position, Tile currentTile, Tile targetTile) : base(position)
        {
            TargetTile = targetTile;
            currentTile.ObjectEntered += CurrentTile_ObjectEntered;
        }

        private void CurrentTile_ObjectEntered(object sender, object e)
        {
            bool activated = e is Theron || e is Creature;

            if (activated)
                Activate();
        }

        protected virtual void Activate()
        {
            TargetTile.ActivateTileContent();
        }
    }
}
