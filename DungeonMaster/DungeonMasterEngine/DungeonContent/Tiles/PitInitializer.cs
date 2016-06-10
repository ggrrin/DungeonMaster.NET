namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class PitInitializer : FloorInitializer
    {
        public new event Initializer<PitInitializer> Initializing;
        public bool IsImaginary { get; set; }

        public bool IsVisible { get; set; }
        
        public bool IsOpen { get; set; }
    }
}