using DungeonMasterEngine.Builders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Player;
using DungeonMasterParser;

namespace DungeonMasterEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Dungeon dungeon;
        private readonly Queue<Tuple<SendOrPostCallback, object>> taskQueue = new Queue<Tuple<SendOrPostCallback, object>>();

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
                IsFullScreen = false
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            SynchronizationContext.SetSynchronizationContext(new GameSynchronizationContext(taskQueue));


            Debug.WriteLine(id);
        }

        public static readonly int id = Thread.CurrentThread.ManagedThreadId;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceProvider.Instance.Initialize(GraphicsDevice, Content);
            var dungeonParser = new DungeonParser();
            dungeonParser.Parse();
            var renderers = new DefaulRenderers(Content, GraphicsDevice);
            var factoreis = new LegacyFactories(dungeonParser.Data, renderers);
            var theron = new LegacyLeader(factoreis);
            dungeon = new Dungeon(new LegacyMapBuilder(dungeonParser.Data, renderers), factoreis, theron, GraphicsDevice);
            GameConsole.InitializeConsole(this, dungeon);
            GameConsole.Instance.DrawOrder = 1;
        }

        protected override void Update(GameTime gameTime)
        {



            dungeon?.Update(gameTime);
            while (taskQueue.Count > 0)
            {
                Tuple<SendOrPostCallback, object> task;
                lock (taskQueue)
                    task = taskQueue.Dequeue();

                task.Item1(task.Item2);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(dungeon?.Effect.FogColor ?? Color.CornflowerBlue.ToVector3()));
            dungeon?.Draw(gameTime);

            base.Draw(gameTime);
        }

    }
}
