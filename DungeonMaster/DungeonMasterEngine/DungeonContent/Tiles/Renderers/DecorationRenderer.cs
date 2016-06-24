using DungeonMasterEngine.DungeonContent.Actuators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class DecorationRenderer<TDecoration> : TextureRenderer where TDecoration : IActuatorX
    {
        public TDecoration Item { get; }

        public DecorationRenderer(Texture2D decorationTexture, TDecoration item) : base(Matrix.CreateScale(0.33f) * Matrix.CreateTranslation(-Vector3.UnitZ * 0.499f), decorationTexture)
        {
            Item = item;
        }


    }
}