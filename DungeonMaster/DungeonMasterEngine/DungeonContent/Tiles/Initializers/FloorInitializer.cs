using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public class FloorInitializer : TileInitializer
    {
        public new event Initializer<FloorInitializer> Initializing;
        public new event Initializer<FloorInitializer> Initialized;

        public FloorTileSide FloorSide { get; set; }
        public IEnumerable<TileSide> WallSides { get; set; }

        protected override void OnInitializing()
        {
            base.OnInitializing();
            Initializing?.Invoke(this);
        }

        protected override void OnInitialized()
        {
            Initialized?.Invoke(this);
            base.OnInitialized();
        }
    }
}