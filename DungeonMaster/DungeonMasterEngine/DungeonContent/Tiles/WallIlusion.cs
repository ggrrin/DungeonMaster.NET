using System;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class WallIlusion : WallIlusion<Message>
    {
        public WallIlusion(WallIlusionInitializer initalizer) : base(initalizer) { }
    }

    public class WallIlusion<TMessage> : FloorTile<TMessage> where TMessage : Message
    {
        public bool IsImaginary { get; private set; }
        public bool IsOpen { get; private set; }
        public bool RandomDecoration { get; private set; }

        public override bool IsAccessible => IsImaginary || IsOpen;

        public WallIlusion(WallIlusionInitializer initalizer) : base(initalizer)
        {
            initalizer.Initializing += Initialize;
        }

        private void Initialize(WallIlusionInitializer initializer)
        {
            IsImaginary = initializer.Imaginary;
            IsOpen = initializer.Open;
            RandomDecoration = initializer.RandomDecoration;

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
