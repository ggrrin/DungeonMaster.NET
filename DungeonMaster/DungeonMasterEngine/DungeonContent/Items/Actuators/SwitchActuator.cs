using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class SwitchActuator : Actuator
    {
        public Tile TargetTile { get; }

        public bool IsOn { get; private set; }

        public SwitchActuator(Vector3 position, Tile targetTile, bool isOn) : base(position)
        {
            TargetTile = targetTile;
            IsOn = isOn;
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (IsOn = !IsOn)
                TargetTile.ActivateTileContent();
            else
                TargetTile.DeactivateTileContent();

            return base.ExchangeItems(item);
        }
    }
}
