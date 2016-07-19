using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders.ItemCreators
{
    public class PotionInitializer : IPotionInitializer {
        public int PotionPower { get; set; }
        public PotionFactory Factory { get; set; }
    }
}