using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Initializers
{
    public interface IPotionInitializer
    {
        int PotionPower { get; }
        PotionFactory Factory { get; set; }
    }
}