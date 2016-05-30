using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders
{
    public class ClothInitializator : IClothInitializer {
        public bool IsBroken { get; set; }
        public bool IsCruised { get; set; }
    }
}