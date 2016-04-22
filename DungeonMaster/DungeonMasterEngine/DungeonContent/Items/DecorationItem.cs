using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public class DecorationItem : Actuator 
    {
        public DecorationItem(Vector3 position) : base(position)
        {}

        public DecorationItem(Vector3 position, Texture2D decoration) : base(position)
        {
            ((CubeGraphic) Graphics).Texture = decoration;
        }
    }
}