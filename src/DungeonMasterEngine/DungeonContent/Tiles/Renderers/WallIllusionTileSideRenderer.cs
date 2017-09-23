using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class WallIllusionTileSideRenderer : TileWallSideRenderer<TileSide>
    {
        private readonly Matrix outerWallTransformation = Matrix.CreateTranslation(Vector3.UnitZ * 1.001f) * Matrix.CreateRotationY(MathHelper.Pi);
        private readonly DecorationRenderer<DecorationItem> decorationRenderer;
        private readonly bool drawDecoration;  

        public WallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decoration) : base(tileSide, wallTexture)
        {
            decorationRenderer = new DecorationRenderer<DecorationItem>(decoration, new DecorationItem());
            drawDecoration = decoration != null;
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var baseTransformation = base.Render(ref currentTransformation, effect, parameter);

            var finalTransformation = outerWallTransformation * baseTransformation;
            RenderWall(effect, ref finalTransformation);

            if(drawDecoration)
                decorationRenderer.Render(ref finalTransformation, effect, parameter);

            return baseTransformation;
        }
    }
}