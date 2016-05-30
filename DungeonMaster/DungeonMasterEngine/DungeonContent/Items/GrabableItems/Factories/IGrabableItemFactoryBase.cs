using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.Attacks;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public interface IGrabableItemFactoryBase
    {
        //Texture2D FloorGraphicsIndex { get; }
        string Name { get; }
        float Weight { get; }
        IEnumerable<IAttackFactory> AttackCombo { get; }
        IEnumerable<IStorageType> CarryLocation { get; }
        bool CanBeStoredIn(IStorageType storage);
    }
}