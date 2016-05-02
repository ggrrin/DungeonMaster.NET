using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent
{
    public class Dungeon : DrawableGameComponent
    {
        private readonly BreadthFirstSearch bfs = new BreadthFirstSearch();
        private List<Tile> currentVisibleTiles;
        private SpriteBatch batcher;

        public DungeonLevel CurrentLevel { get; private set; }

        public Theron Theron { get; }

        public BasicEffect Effect { get; private set; }

        public IPlayer CurrentPlayer => Theron;

        public IDungonBuilder Builder { get; }

        public LevelCollection ActiveLevels { get; }

        public int FogHorizont { get; } = 8;

        public GameTime Time { get; private set; }

        public Dungeon(Game game, IDungonBuilder builder) : base(game)
        {
            InitializeGraphics();
            Game.Components.Add(this);
            Builder = builder;

            Theron = new Theron(Game);
            Game.Components.Add(Theron);
            CurrentPlayer.LocationChanged += CurrentPlayer_LocationChanged;

            ActiveLevels = new LevelCollection();
            var l = LoadLevel(3, new Point(7, 3)); 
            CurrentPlayer.Location = l.StartTile;
            EnabledChanged += Dungeon_EnabledChanged;
        }

        private void Dungeon_EnabledChanged(object sender, EventArgs e)
        {
            Theron.Enabled = Enabled;
        }


        private DungeonLevel LoadLevel(int levelIndex, Point? enterTile)
        {
            var nextLevel = Builder.GetLevel(levelIndex, this, enterTile);
            ActiveLevels.Add(nextLevel);
            return nextLevel;
        }

        private void ConnectLevels(ILevelConnector e)
        {
            DungeonLevel nextLevel;
            if (!ActiveLevels.Contains(e.NextLevelIndex, out nextLevel))
                nextLevel = LoadLevel(e.NextLevelIndex, e.TargetTilePosition);//load level if necesarry    

            e.NextLevelEnter = nextLevel.TilesPositions[e.TargetTilePosition];//TODO unolad level disconect

            UpdateVisibleTiles();
        }

        private void CurrentPlayer_LocationChanged(object sender, EventArgs e)
        {
            if (Time != null)//not on an initialization
            {
                UpdateVisibleTiles();

                foreach (var t in currentVisibleTiles)
                {
                    var connector = t as ILevelConnector;
                    if (connector != null)
                        ConnectLevels(connector);
                }

                CurrentLevel = ActiveLevels.Single(x => x.LevelIndex == Theron.Location.LevelIndex);
            }
        }


        private void InitializeGraphics()
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
                EmissiveColor = Vector3.UnitX,
                FogColor = Vector3.Zero,
                FogEnabled = true,
                FogStart = 0,
                FogEnd = FogHorizont
            };
            Effect.EnableDefaultLighting();
        }


        public override void Update(GameTime gameTime)
        {
            Time = gameTime;
            if (currentVisibleTiles == null)
                CurrentPlayer_LocationChanged(this, new EventArgs());

            Effect.World = Matrix.Identity;
            Effect.View = Theron.View;
            Effect.Projection = Theron.Projection;

            foreach (var tile in CurrentLevel.Tiles)
            {
                tile.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void UpdateVisibleTiles()
        {
            currentVisibleTiles = new List<Tile>();
            bfs.StartSearch(CurrentPlayer.Location, (FogHorizont + 1) * 2 + 1, (t) => currentVisibleTiles.Add(t));
        }

        private Texture2D pixel;



        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);



            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            foreach (var t in currentVisibleTiles.ReverseLazy())
                t.GraphicsProvider?.Draw(Effect);

            DrawMiniMap();
        }

        private void DrawMiniMap()
        {
            if (CurrentLevel != null)
            {
                const int scale = 8;
                batcher.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                batcher.Draw(CurrentLevel.MiniMap, new Rectangle(Point.Zero, new Point(CurrentLevel.MiniMap.Width * scale, CurrentLevel.MiniMap.Height * scale)), Color.White);
                batcher.Draw(pixel, new Rectangle((CurrentPlayer.Location.GridPosition.ToVector2() * scale).ToPoint(), new Point(scale, scale)), Color.White);
                batcher.End();
            }
        }
    }
}
