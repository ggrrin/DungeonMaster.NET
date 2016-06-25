using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
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

        public override string ToString() => $"Scrool \"{Text}\"";
    }
}