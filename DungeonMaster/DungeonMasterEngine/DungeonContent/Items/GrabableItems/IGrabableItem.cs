using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
{
    public interface IGrabableItem : IRenderable, ILocalizable<ITile>
    {
        IGrabableItemFactoryBase Factory { get; }

        void SetLocationNoEvents(ITile tile);
    }
}