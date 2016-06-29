using System;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics;
using DungeonMasterParser.Enums;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class DoorTile : DoorTile<Message>
    {
        public DoorTile(DoorInitializer initializer) : base(initializer) { }
    }

    public class DoorTile<TMessage> : FloorTile<TMessage>, IHasEntity where TMessage : Message
    {
        public Door Door { get; private set; }
        public IEntity Entity =>  Door.Open ? (IEntity)null : Door;

        public bool HasButton { get; private set; }
        public MapDirection Direction { get; private set; }

        public override bool ContentActivated
        {
            get { return Door.Open; }
            protected set { Door.Open = value; }
        }

        public override bool IsAccessible => ContentActivated;

        public DoorTile(DoorInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(DoorInitializer initializer)
        {
            Direction = initializer.Direction;
            Door = initializer.Door;
            HasButton = initializer.HasButton;

            initializer.Initializing -= Initialize;
        }

    }
}
