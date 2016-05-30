using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
{
    public class Cloth : GrabableItem
    {
        private readonly ClothItemFactory clothItemFactory;

        public override IGrabableItemFactoryBase Factory => clothItemFactory;

        public bool IsBroken { get; }
        public bool IsCruised{ get; }


        public Cloth(IClothInitializer initializer, ClothItemFactory clothItemFactory)
        {
            IsBroken = initializer.IsBroken;
            IsCruised = initializer.IsCruised;
            this.clothItemFactory = clothItemFactory;
        }


    }
}