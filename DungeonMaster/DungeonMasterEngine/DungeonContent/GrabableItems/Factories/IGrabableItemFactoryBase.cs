using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public interface IGrabableItemFactoryBase
    {
        string Name { get; }
        int Weight { get; }
        IEnumerable<IActionFactory> ActionCombo { get; }
        IEnumerable<IStorageType> CarryLocation { get; }
        bool CanBeStoredIn(IStorageType storage);
        IRenderer Renderer { get; }


        IGrabableItem CreateItem();
    }
}