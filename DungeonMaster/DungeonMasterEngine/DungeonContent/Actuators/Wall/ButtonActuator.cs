using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class Button : RemoteActuator 
    {
        public Texture2D Texture
        {
            get { return ((CubeGraphic) Graphics).Texture; }
            set { ((CubeGraphic) Graphics).Texture = value; }
        }

        public override ActionStateX TargetAction { get; }

        public Button(Vector3 position, Tile targetTile, ActionStateX action) : base(targetTile, position)
        {
            TargetAction = action;
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            SendMessageAsync();
            return base.ExchangeItems(item);
        }
    }
}