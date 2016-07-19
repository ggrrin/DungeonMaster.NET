using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders.ItemCreators
{
    public class ClothInitializer : IClothInitializer {
        public bool IsBroken { get; set; }
        public bool IsCruised { get; set; }
    }
}