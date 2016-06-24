using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class FloorTile : FloorTile<Message>
    {
        public FloorTile(FloorInitializer initializer ) : base(initializer) {}
    }

    public class FloorTile<TMessage> : Tile<TMessage> where TMessage : Message
    {
        public FloorTileSide FloorSide { get; private set; }
        public override IEnumerable<TileSide> Sides => WallSides.Concat(new [] {FloorSide});
        public override bool IsAccessible => true;

        public FloorTile(FloorInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(FloorInitializer initializer)
        {
            FloorSide = initializer.FloorSide;
            WallSides = initializer.WallSides;

            initializer.Initializing -= Initialize;
        }

        public override IEnumerable<object> SubItems => FloorSide;

        public IEnumerable<TileSide> WallSides { get; private set; }

        public override void OnObjectEntered(object localizable)
        {
            base.OnObjectEntered(localizable);
            FloorSide.OnObjectEntered(localizable);
        }

        public override void OnObjectLeft(object localizable)
        {
            FloorSide.OnObjectLeft(localizable);
            base.OnObjectLeft(localizable);
        }
    }
}
