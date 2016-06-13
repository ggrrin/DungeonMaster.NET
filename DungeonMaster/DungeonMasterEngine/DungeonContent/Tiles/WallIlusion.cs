using System;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class WallIlusion: WallIlusion<Message>
    {
        public WallIlusion(WallIlusionInitializer initalizer) : base(initalizer) {}
    }

    public class WallIlusion<TMessage> : FloorTile<TMessage> where TMessage : Message
    {
        public bool IsImaginary { get; }

        public bool IsOpen { get; private set; }

        public override bool IsAccessible => IsImaginary || IsOpen;

        public WallIlusion(WallIlusionInitializer initalizer) : base(initalizer)
        {
            initalizer.Initializing += Initialize;
        }

        private void Initialize(WallIlusionInitializer initializer)
        {
            //TODO initalization
            initializer.Initializing -= Initialize;
        }

        public override void ActivateTileContent()
        {
            base.ActivateTileContent();
            IsOpen = true;
        }

        public override void DeactivateTileContent()
        {
            base.DeactivateTileContent();
            IsOpen = false;
        }

    }
}
