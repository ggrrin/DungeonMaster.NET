using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders.ItemCreator
{
    public class ClothInitializer : IClothInitializer {
        public bool IsBroken { get; set; }
        public bool IsCruised { get; set; }
    }
}