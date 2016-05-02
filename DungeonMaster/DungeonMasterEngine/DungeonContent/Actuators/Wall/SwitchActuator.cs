using System;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class SwitchActuator : SimpleRemoteActuator
    {

        public SwitchActuator(Vector3 position, Tile targetTile, ActionStateX action ) : base(targetTile, action, position)
        { }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            SendMessageAsync(activated:true); 
            return base.ExchangeItems(item);
        }
    }
}
