using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    public interface IWallGraphicSource
    {
        Renderer GetWallSideRenderer(TileSide side, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetActuatorWallSideRenderer(ActuatorWallTileSide side, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetWallActuatorRenderer(WallActuator res);

        Renderer GetAlcoveDecoration(Alcove alcove, Texture2D wallTexture);

        Renderer GetDecorationRenderer(DecorationItem decoration, Texture2D wallTexture);

        Renderer GetFountainDecoration(Fountain fountain, Texture2D texture);

        Renderer GetTileRenderer(Tile tile);

        Renderer GetCeelingRenderer(TileSide ceeling, Texture2D wallTexture);

        Renderer GetFloorRenderer(FloorTileSide floorTile, Texture2D wallTexture, Texture2D decorationTexture);

        Renderer GetItemRenderer(IGrabableItem item, Texture2D texture2D);

        Renderer GetDoorTileRenderer(DoorTile doorTile, Texture2D frameTexture, Texture2D buttonTexture);

        Renderer GetDoorRenderer(Texture2D doorTexture);

        Renderer GetTextSideRenderer(TextTileSide textTileSide, Texture2D wallTexture);


        Renderer GetChampionRenderer(ChampionDecoration graphics, Texture2D mirror, Texture2D face);
        Renderer GetChampionRenderer(Champion res, Texture2D face);

        Renderer GetTeleportRenderer(TeleportTile teleportTile, Texture2D teleportTexture);

        Renderer GetActuatorFloorRenderer(ActuatorFloorTileSide floor, Texture2D wallTexture, Texture2D texture);
    }
}