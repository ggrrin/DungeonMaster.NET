using System.Linq;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
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

        public ItemActuator(Vector3 position, Tile currentTile, Tile targetTile, IConstrain constrain, ActionStateX action) : base(position, currentTile, targetTile, action)
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
