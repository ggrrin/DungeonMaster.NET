using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
{
    public class Weapon : GrabableItem
    {
        public WeaponItemFactory FactoryType { get; }
        public override IGrabableItemFactoryBase Factory => FactoryType; 

        public Weapon(IWeaponInitializer initializer, WeaponItemFactory weaponItemFactory)
        {
            this.FactoryType= weaponItemFactory;
        }

    }
}