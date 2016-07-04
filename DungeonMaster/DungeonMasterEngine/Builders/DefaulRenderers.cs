using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Renderers;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Renderers;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DecorationItem = DungeonMasterEngine.DungeonContent.Actuators.DecorationItem;

namespace DungeonMasterEngine.Builders
{
    public class DefaulRenderers : IRenderersSource
    {
        public virtual  Renderer GetWallSideRenderer(TileSide side, Texture2D wallTexture, Texture2D decorationTexture)
        {
            return new TileWallSideRenderer<TileSide>(side, wallTexture, decorationTexture);
        }

        public virtual  Renderer GetActuatorWallSideRenderer(ActuatorWallTileSide side, Texture2D wallTexture, Texture2D decorationTexture)
        {
            return new ActuatorSideRenderer(side, wallTexture, decorationTexture);
        }

        public virtual  Renderer GetFloorActuatorRenderer(FloorActuator res)
        {
            return new ActuatorRenderer<FloorActuator>(res);
        }

        public virtual  Renderer GetWallActuatorRenderer(WallActuator res)
        {
            return new WallActuatorRenderer(res);
        }

        public virtual  Renderer GetAlcoveDecorationRenderer(Alcove alcove, Texture2D wallTexture)
        {
            return new AlcoveRenderer(wallTexture, alcove);
        }

        public virtual  Renderer GetRandomDecorationRenderer(DecorationItem decoration, Texture2D texture)
        {
            return new DecorationRenderer<DecorationItem>(texture, decoration);
        }

        public virtual  Renderer GetFountainDecoration(Fountain fountain, Texture2D texture)
        {
            return new FountainRenderer(texture, fountain);
        }

        public virtual  Renderer GetTileRenderer(Tile tile)
        {
            return new TileRenderer<Tile>(tile);
        }

        public virtual  Renderer GetCeelingRenderer(TileSide ceeling, Texture2D wallTexture)
        {
            return new TileWallSideRenderer<TileSide>(ceeling, wallTexture, null);
        }

        public virtual  Renderer GetItemRenderer(Texture2D texture2D)
        {
            var transformation = Matrix.CreateScale(0.15f) /** Matrix.CreateTranslation(new Vector3(0, -0.25f, 0.01f))*/;
            return new TextureRenderer(transformation, texture2D);
        }

        public virtual  Renderer GetDoorTileRenderer(DoorTile doorTile, Texture2D frameTexture, Texture2D buttonTexture)
        {
            return new DoorTileRenderer(doorTile, frameTexture, buttonTexture);
        }

        public virtual  Renderer GetDoorRenderer(Texture2D doorTexture)
        {
            return new DoorRenderer(doorTexture);
        }

        public virtual  Renderer GetTextSideRenderer(TextTileSide textTileSide, Texture2D wallTexture)
        {
            return new TextTileSideRenderer(textTileSide, wallTexture);
        }

        public virtual  Renderer GetChampionActuatorRenderer(ChampionDecoration graphics, Texture2D mirror, Texture2D face)
        {
            return new ChampionMirrorRenderer(graphics, mirror, face);
        }

        public virtual  Renderer GetChampionRenderer(Champion res, Texture2D face)
        {
            return new LiveEntityRenderer<Champion>(res, face);
        }

        public virtual  Renderer GetTeleportFloorSideRenderer(FloorTileSide floorTileSide, Texture2D wallTexture, Texture2D teleportTexture)
        {
            return new TeleportFloorTileSideRenderer(floorTileSide, wallTexture, teleportTexture);
        }

        public virtual  Renderer GetActuatorFloorRenderer(ActuatorFloorTileSide floor, Texture2D wallTexture, Texture2D texture)
        {
            return new ActuatorFloorTileSideRenderer(floor, wallTexture, texture); 
        }

        public virtual  Renderer GetPitTileRenderer(Pit pit)
        {
            return new PitTileRenderer(pit);
        }

        public virtual  Renderer GetStairsTileRenderer(Stairs stairs, Texture2D wallTexture)
        {
            return new StairsRenderer(stairs, wallTexture);
        }

        public virtual  Renderer GetCreatureRenderer(Creature creature, Texture2D texture2D)
        {
            return new CreatureRenderer(creature, texture2D);
        }

        public virtual  Renderer GetFloorRenderer(FloorTileSide floorTile, Texture2D wallTexture, Texture2D decorationTexture)
        {
            return new FloorTileSideRenderer<FloorTileSide>(floorTile, wallTexture, decorationTexture);
        }

        public virtual  Renderer GetWallIllusionTileRenderer(WallIlusion wallIlusion, Texture2D wallTexture)
        {
            return new WallIllusionRenderer(wallIlusion);
        }

        public virtual  Renderer GetWallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decoration)
        {
            return new WallIllusionTileSideRenderer(tileSide, wallTexture, decoration);
        }
    }
}