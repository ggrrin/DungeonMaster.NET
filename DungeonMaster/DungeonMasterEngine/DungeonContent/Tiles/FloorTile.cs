using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Items;
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
        private IEnumerable<TileSide> wallSides;
        public FloorTileSide FloorSide { get; private set; }
        public override IEnumerable<TileSide> Sides => wallSides.Concat(new [] {FloorSide});
        public override bool IsAccessible => true;

        public FloorTile(FloorInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(FloorInitializer initializer)
        {
            FloorSide = initializer.FloorSide;
            wallSides = initializer.WallSides;

            initializer.Initializing -= Initialize;
        }

        public override IEnumerable<object> SubItems => FloorSide;

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
