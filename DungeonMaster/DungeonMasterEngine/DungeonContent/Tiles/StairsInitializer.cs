namespace DungeonMasterEngine.DungeonContent.Tiles
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