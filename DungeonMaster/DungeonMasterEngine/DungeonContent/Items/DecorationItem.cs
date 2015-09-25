using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Items
{
    public class DecorationItem : Item
    {
        public DecorationItem(Vector3 position) : base(position)
        {}

        public DecorationItem(Vector3 position, Texture2D decoration) : base(position)
        {
            (Graphics as CubeGraphic).Texture = decoration;
        }
    }
}