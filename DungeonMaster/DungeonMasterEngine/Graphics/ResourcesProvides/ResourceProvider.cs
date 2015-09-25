using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DungeonMasterEngine.Graphics.ResourcesProvides
{
    public class ResourceProvider
    {
        private static Texture2D defaultTexture;
        private static GraphicsDevice device;
        private static ContentManager content;

        public Texture2D DefaultTexture
        {
            get { InitCheck(); return defaultTexture; }
            protected set { defaultTexture = value; }
        }

        public GraphicsDevice Device
        {
            get { InitCheck(); return device; }
            private set { device = value; }
        }

        public ContentManager Content
        {
            get { InitCheck(); return content; }
            private set { content = value; }
        }

        public static bool IsInitialized { get; private set; }

        private static ResourceProvider instance;

        public static ResourceProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new ResourceProvider();

                return instance;
            }
        }

        public SpriteFont DefaultFont { get; internal set; }

        protected ResourceProvider() { }

        public void Initialize(GraphicsDevice device, ContentManager content)
        {
            Device = device;
            Content = content;
            defaultTexture = content.Load<Texture2D>("Textures/Default");
            DefaultFont = content.Load<SpriteFont>("Fonts/Default");

            IsInitialized = true;
            Initialize();
        }

        protected void InitCheck()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Not initialized!");
        }

        protected virtual void Initialize() { }
    }
}