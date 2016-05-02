using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class Button : SimpleRemoteActuator 
    {
        public Texture2D Texture
        {
            get { return ((CubeGraphic) Graphics).Texture; }
            set { ((CubeGraphic) Graphics).Texture = value; }
        }


        public Button(Vector3 position, Tile targetTile, ActionStateX action) : base(targetTile, action, position)
        { }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            SendMessageAsync(activated: true);
            return base.ExchangeItems(item);
        }
    }
}