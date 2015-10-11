using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonMasterEngine
{
    public class Dungeon : DrawableGameComponent
    {
        private BreadthFirstSearch bfs = new BreadthFirstSearch();

        public Theron Theron { get; }
        private List<Tile> currentVisibleTiles;

        public BasicEffect Effect { get; private set; }

        public IPlayer CurrentPlayer { get { return Theron; } }

        public IDungonBuilder Builder { get; }

        public LevelCollection ActiveLevels { get; private set; }

        public int FogHorizont { get; } = 8;

        public GameTime Time { get; private set; }

        private SpriteBatch batcher;

        public Dungeon(Game game, IDungonBuilder builder) : base(game)
        {
            InitilizeGraphicsResources();
            Game.Components.Add(this);
            Builder = builder;
            
            Theron = new Theron(Game);
            Game.Components.Add(Theron);
            CurrentPlayer.LocationChanged += CurrentPlayer_LocationChanged;

            ActiveLevels = new LevelCollection();
            var l = LoadLevel(1, new Point(14, 32));
            CurrentPlayer.Location = l.StartTile;
            this.EnabledChanged += Dungeon_EnabledChanged;          
        }

        private void Dungeon_EnabledChanged(object sender, EventArgs e)
        {
            Theron.Enabled = Enabled;
        }

        private DungeonLevel currentLevel;

        private DungeonLevel LoadLevel(int levelIndex, Point? enterTile)
        {
            currentLevel = Builder.GetLevel(levelIndex, this, enterTile);
            ActiveLevels.Add(currentLevel);
            return currentLevel;
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
            }
        }

        private void InitilizeGraphicsResources()
        {
            InitializeEffect();

            ResourceProvider.Instance.Initialize(GraphicsDevice, Game.Content);

        }


        private void InitializeEffect()
        {
            

            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[1] { new Color(1f, 0, 0) });

            batcher = new SpriteBatch(GraphicsDevice);
            Effect = new BasicEffect(GraphicsDevice);


            Effect.TextureEnabled = true;

            Effect.EnableDefaultLighting();
            //primitive color
            Effect.AmbientLightColor = new Vector3(0);
            Effect.DiffuseColor = new Vector3(1f);
            Effect.SpecularColor = new Vector3(0);
            Effect.SpecularPower = 0.1f;
            Effect.Alpha = 1f;
            Effect.EmissiveColor = Vector3.UnitX; //pochoden bych dal
            Effect.FogColor = Vector3.Zero;
            Effect.FogEnabled = true;
            Effect.FogStart = 0;
            Effect.FogEnd = FogHorizont;
        }

        public override void Update(GameTime gameTime)
        {
            Time = gameTime;
            if (currentVisibleTiles == null)
                CurrentPlayer_LocationChanged(this, new EventArgs());

            Effect.World = Matrix.Identity;
            Effect.View = Theron.View;
            Effect.Projection = Theron.Projection;

            base.Update(gameTime);
        }

        private void UpdateVisibleTiles()
        {
            currentVisibleTiles = new List<Tile>();
            bfs.StartSearch(CurrentPlayer.Location, (FogHorizont + 1) * 2 + 1, (t) => currentVisibleTiles.Add(t));
        }

        Texture2D pixel;

        

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            foreach (var t in currentVisibleTiles.ReverseLazy())
                t.GraphicsProvider?.Draw(Effect);

            const int scale = 8;

            batcher.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            batcher.Draw(currentLevel.MiniMap, new Rectangle(Point.Zero, new Point(currentLevel.MiniMap.Width * scale, currentLevel.MiniMap.Height * scale) ), Color.White);
            batcher.Draw(pixel, new Rectangle((CurrentPlayer.Location.GridPosition.ToVector2() * scale).ToPoint(), new Point(scale, scale)), Color.White);
            batcher.End();

        }
    }
}
