using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DecorationItem = DungeonMasterEngine.DungeonContent.Actuators.Wall.DecorationItem;

namespace DungeonMasterEngine.Builders
{
    public class DefaultWallGrahicsSource : IWallGraphicSource
    {
        public Renderer GetRenderer(TileSide side, Texture2D wallTexture, Texture2D decorationTexture)
        {
            return new TileWallSideRenderer<TileSide>(side, wallTexture, decorationTexture);
        }

        public Renderer GetRenderer(ActuatorWallTileSide side, Texture2D wallTexture, Texture2D decorationTexture)
        {
            return new ActuatorSideRenderer(side, wallTexture, decorationTexture);
        }

        public Renderer GetRenderer(ActuatorX res)
        {
            return new ActuatorXRenderer(res);
        }

        public Renderer GetAlcoveDecoration(Alcove alcove, Texture2D wallTexture)
        {
            return new AlcoveRenderer(wallTexture, alcove);
        }

        public Renderer GetDecorationRenderer(DecorationItem decoration, Texture2D texture)
        {
            return new DecorationRenderer<DecorationItem>(texture, decoration);
        }

        public Renderer GetFountainDecoration(Fountain fountain, Texture2D texture)
        {
            return new DecorationRenderer<Fountain>(texture, fountain);
        }

        public Renderer GetTileRenderer(Tile tile)
        {
            return new TileRenderer<Tile>(tile);
        }

        public Renderer GetCeelingRenderer(TileSide ceeling, Texture2D wallTexture)
        {
            return new TileWallSideRenderer<TileSide>(ceeling, wallTexture, null);
        }

        public Renderer GetFloorRenderer(FloorTileSide floorTile, Texture2D wallTexture, Texture2D decorationTexture)
        {
            return new FloorTileSideRenderer(floorTile, wallTexture, decorationTexture);
        }

        public Renderer GetItemRenderer(IGrabableItem item, Texture2D texture2D)
        {
            var transformation = Matrix.CreateScale(0.15f) /** Matrix.CreateTranslation(new Vector3(0, -0.25f, 0.01f))*/;
            return new TextureRenderer(transformation, texture2D);
        }

        public Renderer GetDoorTileRenderer(DoorTile doorTile, Texture2D frameTexture, Texture2D buttonTexture)
        {
            return new DoorTileRenderer(doorTile, frameTexture, buttonTexture);
        }

        public Renderer GetDoorRenderer(Texture2D doorTexture)
        {
            return new DoorRenderer(doorTexture);
        }

        public Renderer GetTextSideRenderer(TextTileSide textTileSide, Texture2D wallTexture)
        {
            return new TextTileSideRenderer(textTileSide, wallTexture);
        }
    }
}