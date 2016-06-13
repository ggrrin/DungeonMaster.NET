using System;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{

    public class Pit : Pit<Message>
    {
        public Pit(PitInitializer initializer) : base(initializer) {}
    }

    public class Pit<TMessage> : FloorTile<TMessage> where TMessage : Message
    {
        public bool IsVisible { get; private  set; }
        public bool IsImaginary { get; private set; }

        public bool IsOpen => ContentActivated;

        public override bool IsAccessible => true;

        public override Vector3 StayPoint => IsOpen ? base.StayPoint + Vector3.Down : base.StayPoint;

        public Pit(PitInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(PitInitializer initializer)
        {
            ContentActivated = true;
            IsVisible = initializer.IsVisible;
            IsImaginary = initializer.IsImaginary;


            initializer.Initializing -= Initialize;
        }

    }
}
