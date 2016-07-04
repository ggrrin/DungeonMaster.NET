using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public interface IHasEffect
    {
        bool Used { get; }
        bool ApplyEffect(ILiveEntity entity);
    }
}