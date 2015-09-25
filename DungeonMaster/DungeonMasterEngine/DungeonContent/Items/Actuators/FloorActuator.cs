using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class FloorActuator : Actuator
    {
        public Tile CurrentTile { get; }
        public Tile TargetTile { get; }

        public Action a;
        public bool active;

        public FloorActuator(Vector3 position, Tile currentTile, Tile targetTile) : base(position)
        {
        }
    }
}
