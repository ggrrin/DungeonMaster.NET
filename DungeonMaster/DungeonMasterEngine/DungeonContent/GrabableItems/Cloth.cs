using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public class Cloth : GrabableItem
    {
        public ClothItemFactory ClothItemFactoryBase { get; }

        public override IGrabableItemFactoryBase FactoryBase => ClothItemFactoryBase;

        public bool IsBroken { get; }
        public bool IsCruised{ get; }


        public Cloth(IClothInitializer initializer, ClothItemFactory clothItemFactoryBase)
        {
            IsBroken = initializer.IsBroken;
            IsCruised = initializer.IsCruised;
            this.ClothItemFactoryBase = clothItemFactoryBase;
        }


    }
}