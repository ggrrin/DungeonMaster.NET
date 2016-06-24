using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public class StairsInitializer : TileInitializer
    {
        public event Initializer<StairsInitializer> Initializer; 
        //look direction from stairs
        public MapDirection UpperLevelDirection { get; set; }
        public MapDirection LowerLevelDirection { get; set; }
        public bool Down { get; set; }

        protected override void OnInitialized()
        {
        }
    }
}