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
                Texture = DrawRenderTarget(),
                Scale = new Vector3(0.3f, 0.3f, 0.1f),
                Rotation = isWestEast ? new Vector3(0, MathHelper.PiOver2, 0) : Vector3.Zero
            };
            

        }

        private Texture2D DrawRenderTarget()
        {
            var device = ResourceProvider.Instance.Device;
            RenderTarget2D target = new RenderTarget2D(device, 128, 128);
            SpriteBatch spriteBatch = new SpriteBatch(device);

            // Set the device to the render target
            device.SetRenderTarget(target);

            device.Clear(Color.Gray);

            spriteBatch.Begin();

            spriteBatch.DrawString(ResourceProvider.Instance.DefaultFont, Text , new Vector2(10), Color.White);
            spriteBatch.End();

            // Reset the device to the back buffer
            device.SetRenderTarget(null);

            return target;
        }


    }
}