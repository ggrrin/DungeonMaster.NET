using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class TextTileSideRenderer : TileWallSideRenderer<TextTileSide>
    {
        private readonly TextureRenderer textRenderer;

        public TextTileSideRenderer(TextTileSide tileSide, Texture2D wallTexture) : base(tileSide, wallTexture, null)
        {
            textRenderer = new TextureRenderer(Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(new Vector3(0, 0, -0.499f)),
                ResourceProvider.Instance.DrawRenderTarget(TileSide.Text.Replace('|', '\n'), Color.Transparent, Color.White));
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var finalMatrix = base.Render(ref currentTransformation, effect, parameter);

            if (TileSide.TextVisible)
                textRenderer.Render(ref finalMatrix, effect, parameter);

            return finalMatrix;
        }
    }
}