using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Renderers;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Projectiles;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    public interface IRenderersSource
    {
        ContentManager Content { get; }
        GraphicsDevice Device { get; }

        //tiles
        IRenderer GetTileRenderer(Tile tile);
        IRenderer GetPitTileRenderer(Pit pit1);
        IRenderer GetWallIllusionTileRenderer(WallIlusion wallIlusion, Texture2D wallTexture);
        IRenderer GetUpperStairsTileRenderer(MapDirection upperEntry, MapDirection lowerEntry, Stairs stairs, Texture2D wallTexture);

        //sides
        IRenderer GetActuatorFloorRenderer(ActuatorFloorTileSide floor, Texture2D wallTexture, Texture2D texture);
        IRenderer GetCeelingRenderer(TileSide ceeling, Texture2D wallTexture);
        IRenderer GetFloorRenderer(FloorTileSide floorTile, Texture2D wallTexture, Texture2D decorationTexture);
        IRenderer GetDoorTileRenderer(DoorTile doorTile, Texture2D frameTexture, Texture2D buttonTexture);
        IRenderer GetWallSideRenderer(TileSide side, Texture2D wallTexture);
        IRenderer GetTeleportFloorSideRenderer(FloorTileSide floorTileSide, Texture2D wallTexture, Texture2D teleportTexture);
        IRenderer GetTextSideRenderer(TextTileSide textTileSide, Texture2D wallTexture);
        IRenderer GetActuatorWallSideRenderer(ActuatorWallTileSide side, Texture2D wallTexture);
        IRenderer GetWallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decoration);


        //actuators
        IRenderer GetFloorActuatorRenderer(FloorActuator res);
        IRenderer GetWallActuatorRenderer(WallActuator res);
        IRenderer GetChampionActuatorRenderer(ChampionDecoration graphics, Texture2D mirror, Texture2D face);


        //decorations
        IRenderer GetAlcoveDecorationRenderer(Alcove alcove, Texture2D wallTexture);
        IRenderer GetRandomDecorationRenderer(DecorationItem decoration, Texture2D wallTexture);
        IRenderer GetFountainDecoration(Fountain fountain, Texture2D texture);
        

        //items & entities
        IRenderer GetDoorRenderer(Texture2D doorTexture);
        ITextureRenderer GetItemRenderer(Texture2D texture2D);
        IRenderer GetChampionRenderer(Champion res, Texture2D face);
        Renderer GetCreatureRenderer(Creature creature, Texture2D texture2D);

        IRenderer GetProjectileSpellRenderer(Projectile projectile, Texture2D texture);

        IRenderer GetLowerStairsTileRenderer(Stairs res, Texture2D wallTexture);
    }
}