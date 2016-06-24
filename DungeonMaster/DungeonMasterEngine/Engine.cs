using DungeonMasterEngine.Builders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;
using System;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.Graphics.ResourcesProvides;

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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceProvider.Instance.Initialize(GraphicsDevice, Content);
            dungeon = new Dungeon(new LegacyMapBuilder(), GraphicsDevice);
            GameConsole.InitializeConsole(this, dungeon);
            GameConsole.Instance.DrawOrder = 1;
        }

        protected override void Update(GameTime gameTime)
        {
            dungeon.Update(gameTime);
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
            GraphicsDevice.Clear(new Color(dungeon.Effect.FogColor));
            dungeon.Draw(gameTime);

            base.Draw(gameTime);
        }

    }
}
