using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Renderers;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    public interface IWallGraphicSource
    {
        Renderer GetWallSideRenderer(TileSide side, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetActuatorWallSideRenderer(ActuatorWallTileSide side, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetFloorActuatorRenderer(FloorActuator res);
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

        Renderer GetTeleportFloorSideRenderer(FloorTileSide floorTileSide, Texture2D wallTexture, Texture2D teleportTexture);

        Renderer GetActuatorFloorRenderer(ActuatorFloorTileSide floor, Texture2D wallTexture, Texture2D texture);

        Renderer GetPitTileRenderer(Pit pit1);

        Renderer GetStairsTileRenderer(Stairs stairs, Texture2D wallTexture);

        Renderer GetCreatureRenderer(Creature creature, Texture2D texture2D);

        Renderer GetWallIllusionTileRenderer(WallIlusion wallIlusion, Texture2D wallTexture);

        Renderer GetWallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decoration);
    }
}