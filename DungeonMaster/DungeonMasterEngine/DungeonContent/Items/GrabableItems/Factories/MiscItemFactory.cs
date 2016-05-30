using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public class MiscItemFactory : GrabableItemFactoryBase
    {
        public MiscItemFactory(string name, float weight, IEnumerable<IAttackFactory> attackCombo, IEnumerable<IStorageType> carryLocation) : base(name, weight, attackCombo, carryLocation)
        {
            
        }

        public Miscellaneous Create<TItemInitiator>(TItemInitiator initiator) where TItemInitiator : IMiscInitializer
        {
            return new Miscellaneous(initiator, this);
        }

    }
}