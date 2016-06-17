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
        private ITile location;
        public abstract IGrabableItemFactoryBase Factory { get; }


        public override string ToString()
        {
            return $"{GetType().Name} : {Factory.Name}";
        }

        public Renderer Renderer { get; set; }

        public ITile Location
        {
            get { return location; }
            set
            {
                location?.OnObjectLeft(this);
                location = value;
                location?.OnObjectEntered(this);
            }
        }

        public void SetLocationNoEvents(ITile tile)
        {
            location = tile;
        }

        public MapDirection MapDirection { get; set; }
    }
}
