using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
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