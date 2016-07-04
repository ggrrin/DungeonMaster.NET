using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Misc
{
    public class ChampionBones : Miscellaneous
    {
        public Champion Champion { get; }

        public ChampionBones(IChampionBonesInitializer initializer, ChampionBonesFactory miscItemFactory) : base(initializer, miscItemFactory)
        {
            Champion = initializer.Champion;
        }
    }
}