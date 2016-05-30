using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public class WeaponItemFactory : GrabableItemFactoryBase
    {
        public int? DeltaEnergy { get; }
        public WeaponClass Class { get; }
        public int KineticEnergy { get; }
        public int ShootDamage { get;  }
        public int Strength { get; }

        public WeaponItemFactory(string name, float weight, IEnumerable<IAttackFactory> attackCombo, IEnumerable<IStorageType> carryLocation, int? deltaEnergy, WeaponClass @class, int kineticEnergy, int shootDamage, int strength) : base(name, weight, attackCombo, carryLocation)
        {
            DeltaEnergy = deltaEnergy;
            Class = @class;
            KineticEnergy = kineticEnergy;
            ShootDamage = shootDamage;
            Strength = strength;
        }

        public Weapon Create<TItemInitiator>(TItemInitiator initiator) where TItemInitiator : IWeaponInitializer
        {
            return new Weapon(initiator, this);
        }
    }
}