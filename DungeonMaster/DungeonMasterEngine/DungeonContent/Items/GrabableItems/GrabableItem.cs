using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
{
    public abstract class GrabableItem : IGrabableItem
    {
        public abstract IGrabableItemFactoryBase Factory { get; }


        public override string ToString()
        {
            return $"{GetType().Name} : {Factory.GetType().Name}";
        }

        public Renderer Renderer { get; set; }
    }
}
