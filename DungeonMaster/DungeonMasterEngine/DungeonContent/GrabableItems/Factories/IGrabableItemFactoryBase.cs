using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public interface IGrabableItemFactoryBase
    {
        string Name { get; }
        int Weight { get; }
        IEnumerable<IActionFactory> ActionCombo { get; }
        IEnumerable<IStorageType> CarryLocation { get; }
        bool CanBeStoredIn(IStorageType storage);
        ITextureRenderer Renderer { get; }


        IGrabableItem CreateItem();
    }
}