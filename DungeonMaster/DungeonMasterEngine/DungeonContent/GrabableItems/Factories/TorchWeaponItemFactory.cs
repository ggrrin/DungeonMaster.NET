using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class TorchWeaponItemFactory : WeaponItemFactory
    {
        public TorchWeaponItemFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, int? deltaEnergy, WeaponClass weaponClass, int kineticEnergy, int shootDamage, int strength, ITextureRenderer renderer) : base(name, weight, attackCombo, carryLocation, deltaEnergy, weaponClass, kineticEnergy, shootDamage, strength, renderer)
        { }

        public override Weapon Create<TItemInitiator>(TItemInitiator initiator)
        {
            return new TorchWeapon(initiator,this);
        }
    }
}