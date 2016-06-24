using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class WallIllusionTileSideRenderer : TileWallSideRenderer<TileSide>
    {
        private readonly Matrix outerWallTransformation = Matrix.CreateTranslation(Vector3.UnitZ * 1.001f) * Matrix.CreateRotationY(MathHelper.Pi);

        public WallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decorationTexture) : base(tileSide, wallTexture, decorationTexture)
        {


        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var baseTransformation = base.Render(ref currentTransformation, effect, parameter);

            var finalTransformation = outerWallTransformation * baseTransformation;
            RenderWall(effect, ref finalTransformation);

            if (TileSide.RandomDecoration)
                decorationRenderer.Render(ref finalTransformation, effect, parameter);

            return baseTransformation;
        }
    }
}