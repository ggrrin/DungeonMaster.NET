using System.Collections.Generic;
using DungeonMasterEngine.Builders.Initializators;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public class WeaponItemFactory : GrabableItemFactoryBase
    {
        public int? DeltaEnergy { get; }
        public WeaponClass Class { get; }
        public int KineticEnergy { get; }
        public int ShootDamage { get;  }
        public int Strength { get; }

        public WeaponItemFactory(string name, int weight, IEnumerable<IAttackFactory> attackCombo, IEnumerable<IStorageType> carryLocation, int? deltaEnergy, WeaponClass @class, int kineticEnergy, int shootDamage, int strength, Texture2D texture) : base(name, weight, attackCombo, carryLocation, texture)
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

        public override IGrabableItem Create()
        {
            return Create(new WeaponInitializator
            {
                ChargeCount = 15
            });

        }
    }
}