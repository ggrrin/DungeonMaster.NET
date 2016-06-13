using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public interface IGrabableItemFactoryBase
    {
        Texture2D Texture { get; }
        string Name { get; }
        int Weight { get; }
        IEnumerable<IAttackFactory> AttackCombo { get; }
        IEnumerable<IStorageType> CarryLocation { get; }
        bool CanBeStoredIn(IStorageType storage);


        IGrabableItem Create();
    }
}