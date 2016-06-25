using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class WallIllusionRenderer : TileRenderer<WallIlusion>
    {

        public WallIllusionRenderer(WallIlusion tile) : base(tile)
        {

        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Matrix finalTransformation;
            if (Tile.IsOpen)
            {
                finalTransformation = GetCurrentTransformation(ref currentTransformation);
                foreach (var side in Tile.Sides.Where(x => !(x.Renderer is WallIllusionTileSideRenderer)))
                {
                    var renderer = side.Renderer;
                    renderer.Highlighted = Highlighted;
                    renderer.Render(ref finalTransformation, effect, parameter);
                }
            }
            else
            {
                finalTransformation = base.Render(ref currentTransformation, effect, parameter);
            }

            return finalTransformation;
        }
    }
}