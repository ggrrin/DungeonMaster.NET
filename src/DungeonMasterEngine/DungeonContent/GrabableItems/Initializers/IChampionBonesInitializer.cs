using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Initializers
{
    public interface IChampionBonesInitializer : IMiscInitializer
    {
        Champion Champion { get; }
    }
}