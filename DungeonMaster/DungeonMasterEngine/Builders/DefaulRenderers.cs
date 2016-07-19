using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Renderers;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Renderers;
using DungeonMasterEngine.DungeonContent.Projectiles;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using DecorationItem = DungeonMasterEngine.DungeonContent.Actuators.DecorationItem;

namespace DungeonMasterEngine.Builders
{
    public class DefaulRenderers : IRenderersSource
    {

        public ContentManager Content { get; }
        public GraphicsDevice Device { get; }

        public DefaulRenderers(ContentManager content, GraphicsDevice device)
        {
            Content = content;
            Device = device;
        }

        public virtual IRenderer GetWallSideRenderer(TileSide side, Texture2D wallTexture)
        {
            return new TileWallSideRenderer<TileSide>(side, wallTexture);
        }

        public virtual IRenderer GetActuatorWallSideRenderer(ActuatorWallTileSide side, Texture2D wallTexture )
        {
            return new ActuatorSideRenderer(side, wallTexture);
        }

        public virtual IRenderer GetFloorActuatorRenderer(FloorActuator res)
        {
            return new ActuatorRenderer<FloorActuator>(res);
        }

        public virtual IRenderer GetWallActuatorRenderer(WallActuator res)
        {
            return new WallActuatorRenderer(res);
        }

        public virtual IRenderer GetAlcoveDecorationRenderer(Alcove alcove, Texture2D wallTexture)
        {
            return new AlcoveRenderer(wallTexture, alcove);
        }

        public virtual IRenderer GetRandomDecorationRenderer(DecorationItem decoration, Texture2D texture)
        {
            return new DecorationRenderer<DecorationItem>(texture, decoration);
        }

        public virtual IRenderer GetFountainDecoration(Fountain fountain, Texture2D texture)
        {
            return new FountainRenderer(texture, fountain);
        }


        public virtual IRenderer GetTileRenderer(Tile tile)
        {
            return new TileRenderer<Tile>(tile);
        }

        public virtual IRenderer GetCeelingRenderer(TileSide ceeling, Texture2D wallTexture)
        {
            return new TileWallSideRenderer<TileSide>(ceeling, wallTexture);
        }

        public virtual ITextureRenderer GetItemRenderer(Texture2D texture2D)
        {
            var transformation = Matrix.CreateScale(0.15f) /** Matrix.CreateTranslation(new Vector3(0, -0.25f, 0.01f))*/;
            return new TextureRenderer(transformation, texture2D);
        }

        public virtual IRenderer GetDoorTileRenderer(DoorTile doorTile, Texture2D frameTexture, Texture2D buttonTexture)
        {
            return new DoorTileRenderer(doorTile, frameTexture, buttonTexture);
        }

        public virtual IRenderer GetDoorRenderer(Texture2D doorTexture)
        {
            return new DoorRenderer(doorTexture);
        }

        public virtual IRenderer GetTextSideRenderer(TextTileSide textTileSide, Texture2D wallTexture)
        {
            return new TextTileSideRenderer(textTileSide, wallTexture);
        }

        public virtual IRenderer GetChampionActuatorRenderer(ChampionDecoration graphics, Texture2D mirror, Texture2D face)
        {
            return new ChampionMirrorRenderer(graphics, mirror, face);
        }

        public virtual IRenderer GetChampionRenderer(Champion res, Texture2D face)
        {
            return new MovableRenderer<Champion>(res, face);
        }

        public virtual IRenderer GetTeleportFloorSideRenderer(FloorTileSide floorTileSide, Texture2D wallTexture, Texture2D teleportTexture)
        {
            return new TeleportFloorTileSideRenderer(floorTileSide, wallTexture, teleportTexture);
        }

        public virtual IRenderer GetActuatorFloorRenderer(ActuatorFloorTileSide floor, Texture2D wallTexture, Texture2D texture)
        {
            return new ActuatorFloorTileSideRenderer(floor, wallTexture, texture);
        }

        public virtual IRenderer GetPitTileRenderer(Pit pit)
        {
            return new PitTileRenderer(pit);
        }

        public virtual IRenderer GetStairsTileRenderer(Stairs stairs, Texture2D wallTexture)
        {
            return new StairsRenderer(stairs, wallTexture);
        }

        public virtual Renderer GetCreatureRenderer(Creature creature, Texture2D texture2D)
        {
            return new CreatureRenderer(creature, texture2D);
        }

        public IRenderer GetProjectileSpellRenderer(Projectile projectile, Texture2D texture)
        {
            return new MovableRenderer<Projectile>(projectile, texture);
        }

        public virtual IRenderer GetFloorRenderer(FloorTileSide floorTile, Texture2D wallTexture, Texture2D decorationTexture)
        {
            return new FloorTileSideRenderer<FloorTileSide>(floorTile, wallTexture, decorationTexture);
        }

        public virtual IRenderer GetWallIllusionTileRenderer(WallIlusion wallIlusion, Texture2D wallTexture)
        {
            return new WallIllusionRenderer(wallIlusion);
        }

        public virtual IRenderer GetWallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decoration)
        {
            return new WallIllusionTileSideRenderer(tileSide, wallTexture, decoration);
        }
    }
}