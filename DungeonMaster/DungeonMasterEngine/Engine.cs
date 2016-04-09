using DungeonMasterEngine.Builders;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;
using System;
using DungeonMasterEngine.GameConsoleContent;
using System.Threading.Tasks;
using System.Diagnostics;
using DungeonMasterEngine.DungeonContent;

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
        private Queue<Tuple<SendOrPostCallback, object>> taskQueue = new Queue<Tuple<SendOrPostCallback, object>>();

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            SynchronizationContext.SetSynchronizationContext(new GameSynchronizationContext(taskQueue));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dungeon = new Dungeon(this, new OldDungeonBuilder()); //new OldDungeonBuilder());     

            dungeon.DrawOrder = 0;
            GameConsole.InitializeConsole(this, dungeon);
            GameConsole.Instance.DrawOrder = 1;             
        }


        protected override void Update(GameTime gameTime)
        {
            while (taskQueue.Count > 0)
            {
                var task = taskQueue.Dequeue();
                task.Item1(task.Item2);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(dungeon.Effect.FogColor));

            base.Draw(gameTime);
        }

    }
}
