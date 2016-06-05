using System;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class WallIlusion: WallIlusion<Message>
    {
        public WallIlusion(WallIlusionInitializer initalizer) : base(initalizer) {}
    }

    public class WallIlusionInitializer : FloorInitializer 
    {
        public new event Initializer<WallIlusionInitializer> Initializing;

        public bool Imaginary { get; set; }
        public bool Open { get; set; }
    }


    public class WallIlusion<TMessage> : Floor<TMessage> where TMessage : Message
    {
        public bool IsImaginary { get; } 

        private bool isOpen;
        public bool IsOpen
        {
            get { return isOpen; }
            private set
            {
                isOpen = value;
                UpdateWall();
            }
        }

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

        protected override void UpdateWall()
        {
            //base.UpdateWall();
            //wallGraphic.Outter = !IsOpen;
            //if (!IsOpen)
            //    wallGraphic.DrawFaces |= CubeFaces.Sides;
        }

    }
}
