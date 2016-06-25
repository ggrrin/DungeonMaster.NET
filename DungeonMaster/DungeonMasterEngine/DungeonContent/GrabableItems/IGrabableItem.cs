using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public interface IGrabableItem : IRenderable, ILocalizable<ITile>
    {
        IGrabableItemFactoryBase Factory { get; }

        void SetLocationNoEvents(ITile tile);
    }
}