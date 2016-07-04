using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders.ItemCreator
{
    public class ChampionBonesInitializer : MiscInitializer , IChampionBonesInitializer
    {
        public Champion Champion { get; set; }
    }
}