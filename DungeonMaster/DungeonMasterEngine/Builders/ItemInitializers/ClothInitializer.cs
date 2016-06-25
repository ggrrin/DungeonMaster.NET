using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders.ItemInitializers
{
    public class ClothInitializer : IClothInitializer {
        public bool IsBroken { get; set; }
        public bool IsCruised { get; set; }
    }
}