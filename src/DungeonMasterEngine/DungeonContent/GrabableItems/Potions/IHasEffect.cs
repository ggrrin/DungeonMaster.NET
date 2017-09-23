using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public interface IHasEffect
    {
        bool Used { get; }
        string Message { get; }

        bool ApplyEffect(ILiveEntity entity);

        IGrabableItem GetUsedOutcomeItem(IFactories factories);
    }
}