using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public abstract class TileInitializer : InitializerBase
    {
        public event Initializer<TileInitializer> Initializing;

        public DungeonLevel Level { get; set; }
        public Point GridPosition { get; set; }
        public TileNeighbours Neighbours { get; set; }

        protected override void OnInitialize()
        {
            Initializing?.Invoke(this);
        }
    }
}