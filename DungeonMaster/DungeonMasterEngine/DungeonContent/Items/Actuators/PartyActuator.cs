using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Player;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    enum ActionState
    {
        Set = 0, Clear, Toggle, Hold
    }

    public abstract class PartyActuator : Actuator
    {
        public bool Enabled { get; private set; } = true;

        public Tile TargetTile { get; }

        public PartyActuator(Vector3 position, Tile tile, Tile targetTile) : base(position)
        {
            TargetTile = targetTile;
            tile.ObjectEntered += Tile_ObjectEntered;
        }

        private void Tile_ObjectEntered(object sender, object e)
        {
            var theron = (e as Theron);

            if (Enabled && theron?.PartyGroup.Count == 4)
            {
                Activate(theron);   
            }
        }

        protected abstract void Activate(Theron theron);
    }
}
