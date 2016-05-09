using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public class TextTag : DecorationItem
    {
        public string Text { get; }

       
        public TextTag(Vector3 position, bool visible, bool isWestEast, string text) : base(position)
        {
            Text = text;
            Visible = visible;


            Graphics = new CubeGraphic
            {
                Position = Position,
                Outter =  true,
                Texture = ResourceProvider.Instance.DrawRenderTarget(Text, Color.Black, Color.White),
                Scale = new Vector3(0.3f, 0.3f, 0.1f),
                Rotation = isWestEast ? new Vector3(0, MathHelper.PiOver2, 0) : Vector3.Zero
            };
            

        }

    }
}