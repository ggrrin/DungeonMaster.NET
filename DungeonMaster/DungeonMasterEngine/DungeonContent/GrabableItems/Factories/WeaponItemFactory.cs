using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemInitializers;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class WeaponItemFactory : GrabableItemFactoryBase
    {
        public int? DeltaEnergy { get; }
        public WeaponClass Class { get; }
        public int KineticEnergy { get; }
        public int ShootDamage { get;  }
        public int Strength { get; }

        public WeaponItemFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, int? deltaEnergy, WeaponClass @class, int kineticEnergy, int shootDamage, int strength, Texture2D texture) : base(name, weight, attackCombo, carryLocation, texture)
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
            return Create(new WeaponInitializer
            {
                ChargeCount = 15
            });

        }
    }
}