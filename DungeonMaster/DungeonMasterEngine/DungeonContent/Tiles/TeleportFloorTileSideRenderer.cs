using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class TeleportFloorTileSideRenderer : FloorTileSideRenderer<FloorTileSide>
    {
        private readonly TextureRenderer teleport;

        public TeleportFloorTileSideRenderer(FloorTileSide tileSide, Texture2D wallTexture, Texture2D teleportTexture) : base(tileSide, wallTexture, teleportTexture)
        {
            var identity = Matrix.CreateTranslation(new Vector3(0, 0, -0.499f));
            this.teleport = new TextureRenderer(identity, teleportTexture);
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var matrix = base.Render(ref currentTransformation, effect, parameter);

            //if(Tile.Visible)
            teleport.Render(ref matrix, effect, parameter);

            return matrix;
        }
    }
}