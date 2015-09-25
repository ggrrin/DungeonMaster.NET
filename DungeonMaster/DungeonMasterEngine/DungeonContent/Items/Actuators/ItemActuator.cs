using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class ItemActuator : Actuator
    {
        public Tile TargetTile { get; }

        private bool switchedOn = false;
        public bool SwitchedOn
        {
            get { return switchedOn; }
            set
            {
                if (switchedOn != value)
                    TargetTile.ActivateTileContent();//TODO status!!!

                switchedOn = value;
            }
        }

        private Tile currentTile;

        public IConstrain Constrain { get; }

        public ItemActuator(Vector3 position, Tile currentTile, Tile targetTile, IConstrain constrain) : base(position)
        {
            this.currentTile = currentTile;
            TargetTile = targetTile;
            currentTile.ObjectEntered += CurrentTile_ObjectEntered;
            Constrain = constrain;
        }

        private void CurrentTile_ObjectEntered(object sender, object e)
        {
            if (null != (from i in currentTile.SubItems where Constrain.IsAcceptable(i as GrabableItem) select i).FirstOrDefault())
                SwitchedOn = true;
            else
                SwitchedOn = false;
        }
    }
}
