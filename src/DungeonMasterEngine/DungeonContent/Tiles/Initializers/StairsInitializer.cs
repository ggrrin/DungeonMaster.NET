using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public class StairsInitializer : FloorInitializer 
    {
        public new event Initializer<StairsInitializer> Initializing;
        //look direction from stairs
        public MapDirection UpperLevelDirection { get; set; }
        public MapDirection LowerLevelDirection { get; set; }
        public bool Down { get; set; }

        protected override void OnInitializing()
        {
            base.OnInitializing();
            Initializing?.Invoke(this);
        }

    }
}