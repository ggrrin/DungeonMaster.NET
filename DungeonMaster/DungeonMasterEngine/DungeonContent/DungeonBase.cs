using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent
{
    public class Dungeon : DungeonBase<IFactories,Theron>
    {
        public Dungeon(IDungonBuilder<IFactories> builder, IFactories factoreis, Theron leader, GraphicsDevice graphicsDevice) : base(builder, factoreis, leader, graphicsDevice) {}
    }

    public abstract class DungeonBase<TFactories,TLeader> where TFactories : IFactories where TLeader : ILeader
    {
        protected readonly RendererSearcher bfs = new RendererSearcher();
        protected List<ITile> currentVisibleTiles;
        protected SpriteBatch batcher;

        private TLeader leader;
        public TLeader Leader
        {
            get { return leader; }
            protected set
            {
                if (Leader != null)
                {
                    Leader.LocationChanged -= Leader_LocationChanged;
                }
                leader = value;
                Leader.LocationChanged += Leader_LocationChanged;
            }
        }
        public DungeonLevel CurrentLevel { get; protected set; }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                Leader.Enabled = value;
            }
        }

        public BasicEffect Effect { get; protected set; }
        public IDungonBuilder<TFactories> Builder { get; }
        public TFactories Factories { get; }
        public virtual LevelCollection ActiveLevels { get; }
        public int FogHorizont { get; protected set; } = 5;
        public GameTime Time { get; private set; }
        public GraphicsDevice GraphicsDevice { get; }

        public DungeonBase(IDungonBuilder<TFactories> builder, TFactories factoreis, TLeader leader, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            InitializeGraphics();
            Builder = builder;
            Factories = factoreis;

            ActiveLevels = new LevelCollection();
            DungeonLevel level;
            //level = LoadLevel(1, new Point(4,14));
            //level = LoadLevel(1, new Point(7,21));
            level = LoadLevel(0, new Point(9, 7));
            //level = LoadLevel(0, null);// start
            Leader = leader;
            Leader.Location = level.StartTile;
        }

        protected void Leader_LocationChanged(object sender, EventArgs e)
        {
            if (Time != null)//not on an initialization
            {
                UpdateVisibleTiles();
                SetupLevelConnectors();

                CurrentLevel = ActiveLevels.Single(x => x.LevelIndex == Leader.Location.LevelIndex);
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
            var nextLevel = Builder.GetLevel(Factories, levelIndex, enterTile);
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

            //if (Enabled)
            {
                if (currentVisibleTiles == null)
                    Leader_LocationChanged(this, new EventArgs());

                Effect.World = Matrix.Identity;
                Effect.View = Leader.View;
                Effect.Projection = Leader.Projection;

                Leader.Update(gameTime);
                foreach (var tile in CurrentLevel.Tiles) {}

                foreach (var level in ActiveLevels)
                {
                    level.Update(gameTime);
                }
            }
        }

        protected virtual void UpdateVisibleTiles()
        {
            currentVisibleTiles = new List<ITile>();
            bfs.StartSearch(Leader.Location, Leader.Location, FogHorizont, (tile, layer, bundle) => currentVisibleTiles.Add(tile));
        }

        protected Texture2D pixel;
        private bool enabled = true;

        public virtual void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            var mat = Matrix.Identity;
            foreach (var t in currentVisibleTiles.ReverseLazy())
                t.Renderer?.Render(ref mat, Effect, null);

            Leader.Draw(Effect);

            DrawMiniMap();
        }

        protected virtual void DrawMiniMap()
        {
            if (CurrentLevel != null)
            {
                const int scale = 8;
                batcher.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                batcher.Draw(CurrentLevel.MiniMap, new Rectangle(Point.Zero, new Point(CurrentLevel.MiniMap.Width * scale, CurrentLevel.MiniMap.Height * scale)), Color.White);
                batcher.Draw(pixel, new Rectangle((Leader.Location.GridPosition.ToVector2() * scale).ToPoint(), new Point(scale, scale)), Color.White);
                batcher.End();
            }
        }
    }
}