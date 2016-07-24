using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent
{
    public abstract class DungeonBase<TFactories, TLeader> where TFactories : IFactories where TLeader : ILeader
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
        const float FogHorizontMax = 6.5f;
        //FFFFC2
        //0xED, 0xD2, 0x4B

        Vector3 color = new Vector3(0xFF, 0xFF, 0xC2) / 255f;
        public float Light
        {
            get { return Effect.FogEnd; }
            protected set
            {
                float defaultFogEnd = (FogHorizontMax - value);
                var prevFogEnd = Effect.FogEnd;
                Effect.FogEnd = defaultFogEnd / 1.5f;
                Effect.DiffuseColor = color / (value == 0 ? 1 : value);
                Effect.EmissiveColor = Effect.DiffuseColor;

                if (prevFogEnd != Effect.FogEnd)
                    UpdateVisibleTiles();
            }
        }

        public GameTime Time { get; private set; }
        public GraphicsDevice GraphicsDevice { get; }

        public DungeonBase(IDungonBuilder<TFactories> builder, TFactories factoreis, TLeader leader, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            InitializeGraphicDefaults();
            Builder = builder;
            Factories = factoreis;

            ActiveLevels = new LevelCollection();
            DungeonLevel level;
            //level = LoadLevel(0, new Point(4, 15));
            //level = LoadLevel(3, new Point(6, 8));
            //level = LoadLevel(1, new Point(4, 14));
            //level = LoadLevel(1, new Point(7,21));
            level = LoadLevel(0, new Point(9, 7));
            //level = LoadLevel(0, null);// start
            Leader = leader;
            Leader.Location = Leader.Layout.GetSpaceElement(Leader.Layout.AllSpaces.First(), level.StartTile);

        }

        private void Leader_LocationChanged(object sender, EventArgs e)
        {
            if (Time != null)//not on an initialization
            {
                UpdateVisibleTiles();
                SetupLevelConnectors();

                CurrentLevel = ActiveLevels.Single(x => x.LevelIndex == Leader.Location.Tile.LevelIndex);
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

        protected void InitializeGraphicDefaults()
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
                DiffuseColor = color,
                SpecularColor = new Vector3(0),
                SpecularPower = 0.1f,
                Alpha = 1f,
                EmissiveColor = color,
                FogColor = Vector3.Zero,
                FogEnabled = true,
                FogStart = 0,
                FogEnd = 6
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

                CurrentLevel.Update(gameTime);
                //foreach (var level in ActiveLevels)
                //{
                //    level.Update(gameTime);
                //}
                UpdateLight(gameTime);
            }
        }

        protected virtual void UpdateVisibleTiles()
        {
            currentVisibleTiles = new List<ITile>();
            int maxDistance = (int)Light + 2;
            //$"{maxDistance}:{Light}".Dump();
            bfs.StartSearch(Leader.Location.Tile, Leader.Location.Tile, maxDistance, (tile, layer, bundle) => currentVisibleTiles.Add(tile));
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
                batcher.Draw(pixel, new Rectangle((Leader.Location.Tile.GridPosition.ToVector2() * scale).ToPoint(), new Point(scale, scale)), Color.White);
                batcher.End();
            }
        }

                //F337_akzz_INVENTORY_SetDungeonViewPalette()
        void UpdateLight(GameTime time)
        {
            //var hands = new IStorageType[] { ActionHandStorageType.Instance, ReadyHandStorageType.Instance };
            var lightSources = Leader.PartyGroup
                .SelectMany(ch => ch.Body.BodyParts.SelectMany(bp => bp.Storage))
                .OfType<ILightSource>()
                .ToArray();

            foreach (var lightSource in lightSources)
                lightSource.Update(time);


            if (CurrentLevel.Difficulty == 0)
            {
                Light = 0; /* Brightest color palette index */
            }
            else
            {
                /* Get torch light power from both hands of each champion in the party */
                var torchesPowers = lightSources
                    .Select(t => t.LightPower)
                    .ToList();
                /* Sort torch light power values so that the four highest values are in the first four entries in the array L1045_ai_TorchesLightPower in decreasing order. The last four entries contain the smallest values but they are not sorted */

                torchesPowers.Sort();
                torchesPowers.Reverse();

                /* Get total light amount provided by the four torches with the highest light power values and by the fifth torch in the array which may be any one of the four torches with the smallest ligh power values */
                int L1036_i_TotalLightAmount = 0;
                int TorchLightAmountMultiplier = 0;
                foreach (var power in torchesPowers.Take(Math.Min(5, torchesPowers.Count)))
                {
                    int light = Factories.LightPowerToLightAmount[MathHelper.Clamp(power, 0, 15)] >> TorchLightAmountMultiplier;
                    L1036_i_TotalLightAmount += MathHelper.Min(Factories.MaxLight, light);
                    TorchLightAmountMultiplier++;
                }


                L1036_i_TotalLightAmount += Leader.PartyGroup
                    .Select(x => x.GetProperty(PropertyFactory<MagicalLightProperty>.Instance).Value)
                    .Sum();


                /* Select palette corresponding to the total light amount */
                int A1040_pi_LightAmountIndex = 0;
                int A1039_i_PaletteIndex;
                if (L1036_i_TotalLightAmount > 0)
                {
                    A1039_i_PaletteIndex = 0; /* Brightest color palette index */
                    while (Factories.PaletteIndexToLightAmount[A1040_pi_LightAmountIndex++] > L1036_i_TotalLightAmount)
                        A1039_i_PaletteIndex++;
                }
                else
                {
                    A1039_i_PaletteIndex = 5; /* Darkest color palette index */
                }
                Light = A1039_i_PaletteIndex;
            }
        }
    }
}