using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using DungeonMasterEngine.Graphics.ResourcesProvides;

namespace DungeonMasterEngine.Graphics
{
    public enum CubeFaces : byte
    {
        None = 0, Back = 1, Right = 2, Front = 4, Left = 8, Floor = 16, Ceeling = 32,
        Sides = 0x0F, Horizontal = 48, All = 0x3F
    }

    public class CubeGraphic : Graphic
    {
        public CubeFaces DrawFaces { get; set; } = CubeFaces.All;

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                if (value != null)
                    texture = value;
                else
                    texture = Resources.DefaultTexture;
            }
        }

        public CubeGraphic()
        {
            texture = Resources.DefaultTexture;
        }

        public BufferResourceProvider Resources => CubeResources.Instance;

        public bool Outter { get; set; }

        public override void Draw(BasicEffect effect)
        {
            effect.World = transformationMatrix;
            effect.Texture = Texture;
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
                    effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, i*4, 0, Resources.VertexBuffer.VertexCount, (Outter ? 36 : 0) + i*6, 2);
                }
                match <<= 1;
            }
        }
    }

}
