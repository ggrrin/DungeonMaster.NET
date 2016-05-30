using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
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