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
    public class ItemActuator : FloorActuator
    {

        private bool switchedOn = false;
        public bool SwitchedOn
        {
            get { return switchedOn; }
            private set
            {
                if (switchedOn != value)
                    AffectTile();
                switchedOn = value;
            }
        }


        public IConstrain Constrain { get; }

        public ItemActuator(Vector3 position, Tile currentTile, Tile targetTile, IConstrain constrain, ActionState action) : base(position, currentTile, targetTile, action)
        {
            Constrain = constrain;
        }

        protected override void TestAndRun(object enteringObject, bool objectEntered)
        {
            if (null != (from i in CurrentTile.SubItems where Constrain.IsAcceptable(i as GrabableItem) select i).FirstOrDefault())
                SwitchedOn = true;
            else
                SwitchedOn = false;

        }
    }
}
