using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
{
    public abstract class GrabableItem : Item, IGrabableItem
    {
        public abstract IGrabableItemFactoryBase Factory { get; }

        protected GrabableItem() 
        {
            ((CubeGraphic) Graphics).Texture = ResourceProvider.Instance.DrawRenderTarget(this.GetType().Name, Color.Green, Color.White);

        }

        public override string ToString()
        {
            return $"{GetType().Name} : {Factory.GetType().Name}";
        }

    }
}
