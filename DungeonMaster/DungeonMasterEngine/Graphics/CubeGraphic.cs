using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Graphics
{
    public class CubeGraphic : Graphic
    {
        public CubeFaces DrawFaces { get; set; } = CubeFaces.All;

        private Texture2D texture;
        private Texture2D mirroredTexture2D;

        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value ?? Resources.DefaultTexture;
                mirroredTexture2D = GetFlippedTexture(texture);
            }
        }

        public CubeGraphic()
        {
            texture = Resources.DefaultTexture;
            mirroredTexture2D = texture;
        }

        public BufferResourceProvider Resources => CubeResources.Instance;

        public bool Outter { get; set; }

        private Texture2D GetFlippedTexture(Texture2D normalTexture)
        {
            var data = new Color[normalTexture.Height * normalTexture.Width];
            normalTexture.GetData(data);
            for (int m = 0; m < normalTexture.Height; m++)
            {
                int offset = m * texture.Width;
                for (int i = 0; i < normalTexture.Width / 2; i++)
                {
                    int k = normalTexture.Width - i - 1;
                    var temp = data[offset + i];
                    data[offset + i] = data[offset + k];
                    data[offset + k] = temp;
                }
            }
            var res = new Texture2D(ResourceProvider.Instance.Device, normalTexture.Width, normalTexture.Height);
            res.SetData(data);
            return res;
        }

        public override void Draw(BasicEffect effect)
        {
            effect.World = transformationMatrix;
            effect.Texture = Outter ? mirroredTexture2D : texture;
            effect.GraphicsDevice.Indices = Resources.IndexBuffer;
            effect.GraphicsDevice.SetVertexBuffer(Resources.VertexBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawSides(effect);
            }
        }

        public void DrawSides(BasicEffect effect)
        {
            byte match = 1;
            for (int i = 0; i < 7; i++)//assuming 7th bit set to 0!!!
            {
                if (((byte)DrawFaces & match) != 0) //bit set
                {
                    effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, i * 4, 0, Resources.VertexBuffer.VertexCount, (Outter ? 36 : 0) + i * 6, 2);
                }
                match <<= 1;
            }
        }
    }

}
