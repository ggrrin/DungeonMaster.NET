using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class FloorTileRenderer : TileWallSideRenderer<FloorTileSide>
    {
        public FloorTileRenderer(FloorTileSide tileSide, Texture2D wallTexture, Texture2D decorationTexture) : base(tileSide, wallTexture, decorationTexture) {}
    }
}