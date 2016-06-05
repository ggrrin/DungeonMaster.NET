using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;

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

        IGrabableItem Create();
    }
}