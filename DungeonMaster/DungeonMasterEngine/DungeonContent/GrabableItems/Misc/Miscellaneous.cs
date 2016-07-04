using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Misc
{
    public class Miscellaneous : GrabableItem
    {
        public MiscItemFactory TypeFacotry { get; }

        public override IGrabableItemFactoryBase FactoryBase => TypeFacotry;

        public int ChargeCount { get; protected set; }

        public Miscellaneous(IMiscInitializer initializer, MiscItemFactory miscItemFactory)
        {
            TypeFacotry = miscItemFactory;
            ChargeCount = initializer.Attribute;
        }
    }
}