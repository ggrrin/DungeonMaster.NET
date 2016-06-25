using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
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