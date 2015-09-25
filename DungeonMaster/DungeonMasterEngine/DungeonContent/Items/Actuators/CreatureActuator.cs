using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class CreatureActuator : Actuator
    {
        public Tile TargetTile { get; }

        public CreatureActuator(Vector3 position, Tile currentTile, Tile targetTile) : base(position)
        {
            TargetTile = targetTile;

            currentTile.ObjectEntered += CurrentTile_ObjectEntered;
        }

        private void CurrentTile_ObjectEntered(object sender, object e)
        {
            var creature = e as Creature;

            if (creature != null)
                Activate();
        }

        protected virtual void Activate()
        {
            TargetTile.ActivateTileContent();
        }
    }
}
