using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
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

        public WeaponItemFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, int? deltaEnergy, WeaponClass weaponClass, int kineticEnergy, int shootDamage, int strength, IRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer)
        {
            DeltaEnergy = deltaEnergy;
            Class = weaponClass;
            KineticEnergy = kineticEnergy;
            ShootDamage = shootDamage;
            Strength = strength;
        }

        public Weapon Create<TItemInitiator>(TItemInitiator initiator) where TItemInitiator : IWeaponInitializer
        {
            return new Weapon(initiator, this);
        }

        public override IGrabableItem CreateItem()
        {
            return Create(new WeaponInitializer
            {
                ChargeCount = 15
            });

        }
    }
}