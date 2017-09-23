using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public class Scroll : GrabableItem
    {
        private readonly ScrollItemFactory scrollItemFactoryBase;
        public override IGrabableItemFactoryBase FactoryBase => scrollItemFactoryBase;
        public string Text { get; }

        public Scroll(IScrollInitializer initializator, ScrollItemFactory scrollItemFactoryBase)
        {
            this.Text = initializator.Text;
            this.scrollItemFactoryBase = scrollItemFactoryBase;
        }

        public override string ToString() => $"Scrool \"{Text}\"";
    }
}