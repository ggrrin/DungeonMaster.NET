using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public class Potion : GrabableItem
    {
        public PotionItemFactory Type { get; }
        public override IGrabableItemFactoryBase Factory => Type; 
        public int Power { get; }
        public Potion(IPotionInitializer initializer, PotionItemFactory type)
        {
            Power = initializer.PotionPower;
            Type = type;
        }
    }
}