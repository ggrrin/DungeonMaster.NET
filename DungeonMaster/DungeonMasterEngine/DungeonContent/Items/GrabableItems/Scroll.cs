using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
{
    public class Scroll : GrabableItem
    {
        private readonly ScrollItemFactory scrollItemFactory;
        public override IGrabableItemFactoryBase Factory => scrollItemFactory;
        public string Text { get; }

        public Scroll(IScrollInitializer initializator, ScrollItemFactory scrollItemFactory)
        {
            this.Text = initializator.Text;
            this.scrollItemFactory = scrollItemFactory;
        }
    }
}