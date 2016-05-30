using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.Attacks;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public abstract class GrabableItemFactoryBase : IGrabableItemFactoryBase
    {
        private readonly HashSet<IAttackFactory> combos;
        private readonly HashSet<IStorageType> locations;

        public string Name { get; }
        public float Weight { get; }
        public IEnumerable<IAttackFactory> AttackCombo => combos;
        public IEnumerable<IStorageType> CarryLocation => locations;

        protected GrabableItemFactoryBase(string name, float weight, IEnumerable<IAttackFactory> attackCombo, IEnumerable<IStorageType> carryLocation)
        {
            Name = name;
            Weight = weight;
            combos = new HashSet<IAttackFactory>(attackCombo);
            locations = new HashSet<IStorageType>(CarryLocation);
        }

        public bool CanBeStoredIn(IStorageType storage) => locations.Contains(storage);

    }
}