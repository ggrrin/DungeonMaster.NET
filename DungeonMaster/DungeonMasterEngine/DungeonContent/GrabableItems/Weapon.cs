using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public class Weapon : GrabableItem
    {
        public WeaponItemFactory FactoryType { get; }
        public override IGrabableItemFactoryBase FactoryBase => FactoryType; 

        public Weapon(IWeaponInitializer initializer, WeaponItemFactory weaponItemFactory)
        {
            this.FactoryType= weaponItemFactory;
        }

    }
}