using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class SwitchActuator : RemoteActuator
    {

        public SwitchActuator(Vector3 position, Tile targetTile, ActionStateX action ) : base(targetTile, action, position)
        { }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (TargetTile.ContentActivated)
                TargetTile.DeactivateTileContent();
            else
                TargetTile.ActivateTileContent();

            return base.ExchangeItems(item);
        }
    }
}
