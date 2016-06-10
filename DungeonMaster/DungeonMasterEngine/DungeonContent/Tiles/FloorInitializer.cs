using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class FloorInitializer : TileInitializer
    {
        public new event Initializer<FloorInitializer> Initializing;

        public IEnumerable<TileSide> Sides { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Initializing?.Invoke(this);
        }
    }
}