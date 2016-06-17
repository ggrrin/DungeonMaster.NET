using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class FloorInitializer : TileInitializer
    {
        public new event Initializer<FloorInitializer> Initializing;
        public new event Initializer<FloorInitializer> Initialized;

        public FloorTileSide FloorSide { get; set; }
        public IEnumerable<TileSide> WallSides { get; set; }

        protected override void OnInitialing()
        {
            base.OnInitialing();
            Initializing?.Invoke(this);
        }

        protected override void OnInitialized()
        {
            Initialized?.Invoke(this);
            base.OnInitialized();
        }
    }
}