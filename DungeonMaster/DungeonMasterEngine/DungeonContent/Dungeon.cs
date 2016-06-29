using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent
{
    public class Dungeon : DungeonBase<Theron>
    {
        public Dungeon(IDungonBuilder builder, GraphicsDevice graphicDevice) : base(builder, graphicDevice)
        {
            Leader = new Theron( ActiveLevels.LastAddedLevel.StartTile);
        }
    }

    public abstract class DungeonBase
    {
        protected readonly RendererSearcher bfs = new RendererSearcher();
        protected List<ITile> currentVisibleTiles;
        protected SpriteBatch batcher;

        public abstract ILeader LeaderBase { get; }
        public DungeonLevel CurrentLevel { get; protected set; }
        public bool Enabled { get; set; } = true;
        public BasicEffect Effect { get; protected set; }
        public IDungonBuilder Builder { get; }
        public virtual LevelCollection ActiveLevels { get; }
        public int FogHorizont { get; protected set; } = 5;
        public GameTime Time { get; private set; }
        public GraphicsDevice GraphicsDevice { get; }

        public DungeonBase(IDungonBuilder builder, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            InitializeGraphics();
            Builder = builder;

            ActiveLevels = new LevelCollection();
            LoadLevel(0, null);
        }

        protected void Leader_LocationChanged(object sender, EventArgs e)
        {
            if (Time != null)//not on an initialization
            {
                UpdateVisibleTiles();
                SetupLevelConnectors();

                CurrentLevel = ActiveLevels.Single(x => x.LevelIndex == LeaderBase.Location.LevelIndex);
            }
        }

        protected virtual void SetupLevelConnectors()
        {
            foreach (var t in currentVisibleTiles)
            {
                var connector = t as ILevelConnector;
                if (connector != null)
                    ConnectLevels(connector);
            }
        }

        protected virtual DungeonLevel LoadLevel(int levelIndex, Point? enterTile)
        {
            var nextLevel = Builder.GetLevel(levelIndex, enterTile);
            ActiveLevels.Add(nextLevel);
            return nextLevel;
        }

        protected virtual void ConnectLevels(ILevelConnector e)
        {
            DungeonLevel nextLevel;
            if (!ActiveLevels.Contains(e.NextLevelIndex, out nextLevel))
                nextLevel = LoadLevel(e.NextLevelIndex, e.TargetTilePosition);//load level if necesarry    

            e.NextLevelEnter = nextLevel.TilesPositions[e.TargetTilePosition];//TODO unolad level disconect

            UpdateVisibleTiles();
        }

        protected virtual void InitializeGraphics()
        {
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[1] { new Color(1f, 0, 0) });

            batcher = new SpriteBatch(GraphicsDevice);
            Effect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                AmbientLightColor = new Vector3(0),
                DiffuseColor = new Vector3(1f),
                SpecularColor = new Vector3(0),
                SpecularPower = 0.1f,
                Alpha = 1f,
                EmissiveColor = new Vector3(1f),
                FogColor = Vector3.Zero,
                FogEnabled = true,
                FogStart = 0,
                FogEnd = FogHorizont
            };
            Effect.EnableDefaultLighting();
        }


        public virtual void Update(GameTime gameTime)
        {
            Time = gameTime;

            if (Enabled)
            {
                if (currentVisibleTiles == null)
                    Leader_LocationChanged(this, new EventArgs());

                Effect.World = Matrix.Identity;
                Effect.View = LeaderBase.View;
                Effect.Projection = LeaderBase.Projection;

                foreach (var tile in CurrentLevel.Tiles)
                    tile.Update(gameTime);
            }
        }

        protected virtual void UpdateVisibleTiles()
        {
            currentVisibleTiles = new List<ITile>();
            bfs.StartSearch(LeaderBase.Location, LeaderBase.Location, FogHorizont, (tile, layer, bundle) => currentVisibleTiles.Add(tile));
        }

        protected Texture2D pixel;

        public virtual void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            var mat = Matrix.Identity;
            foreach (var t in currentVisibleTiles.ReverseLazy())
                t.Renderer?.Render(ref mat, Effect, null);

            LeaderBase.Draw(Effect);

            DrawMiniMap();
        }

        protected virtual void DrawMiniMap()
        {
            if (CurrentLevel != null)
            {
                const int scale = 8;
                batcher.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                batcher.Draw(CurrentLevel.MiniMap, new Rectangle(Point.Zero, new Point(CurrentLevel.MiniMap.Width * scale, CurrentLevel.MiniMap.Height * scale)), Color.White);
                batcher.Draw(pixel, new Rectangle((LeaderBase.Location.GridPosition.ToVector2() * scale).ToPoint(), new Point(scale, scale)), Color.White);
                batcher.End();
            }
        }
    }
}
