using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public abstract class GrabableItem : IGrabableItem
    {
        private ITile location;
        public abstract IGrabableItemFactoryBase Factory { get; }


        public override string ToString()
        {
            return $"{GetType().Name} : {Factory.Name}";
        }

        public IRenderer Renderer { get; set; }

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
