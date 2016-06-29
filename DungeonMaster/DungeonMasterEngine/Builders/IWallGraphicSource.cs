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
        //tiles
        Renderer GetTileRenderer(Tile tile);
        Renderer GetPitTileRenderer(Pit pit1);
        Renderer GetWallIllusionTileRenderer(WallIlusion wallIlusion, Texture2D wallTexture);
        Renderer GetStairsTileRenderer(Stairs stairs, Texture2D wallTexture);

        //sides
        Renderer GetActuatorFloorRenderer(ActuatorFloorTileSide floor, Texture2D wallTexture, Texture2D texture);
        Renderer GetCeelingRenderer(TileSide ceeling, Texture2D wallTexture);
        Renderer GetFloorRenderer(FloorTileSide floorTile, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetDoorTileRenderer(DoorTile doorTile, Texture2D frameTexture, Texture2D buttonTexture);
        Renderer GetWallSideRenderer(TileSide side, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetTeleportFloorSideRenderer(FloorTileSide floorTileSide, Texture2D wallTexture, Texture2D teleportTexture);
        Renderer GetTextSideRenderer(TextTileSide textTileSide, Texture2D wallTexture);
        Renderer GetActuatorWallSideRenderer(ActuatorWallTileSide side, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetWallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decoration);


        //actuators
        Renderer GetFloorActuatorRenderer(FloorActuator res);
        Renderer GetWallActuatorRenderer(WallActuator res);
        Renderer GetChampionActuatorRenderer(ChampionDecoration graphics, Texture2D mirror, Texture2D face);


        //decorations
        Renderer GetAlcoveDecorationRenderer(Alcove alcove, Texture2D wallTexture);
        Renderer GetRandomDecorationRenderer(DecorationItem decoration, Texture2D wallTexture);
        Renderer GetFountainDecoration(Fountain fountain, Texture2D texture);
        

        //items & entities
        Renderer GetDoorRenderer(Texture2D doorTexture);
        Renderer GetItemRenderer(IGrabableItem item, Texture2D texture2D);
        Renderer GetChampionRenderer(Champion res, Texture2D face);
        Renderer GetCreatureRenderer(Creature creature, Texture2D texture2D);
    }
}