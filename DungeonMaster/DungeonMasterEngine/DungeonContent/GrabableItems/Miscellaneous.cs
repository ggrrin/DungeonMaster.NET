using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public class Miscellaneous : GrabableItem
    {
        public MiscItemFactory TypeFacotry { get; }

        public override IGrabableItemFactoryBase Factory => TypeFacotry; 

        public Miscellaneous(IMiscInitializer initializer, MiscItemFactory miscItemFactory)
        {
            this.TypeFacotry = miscItemFactory;

        }
    }
}