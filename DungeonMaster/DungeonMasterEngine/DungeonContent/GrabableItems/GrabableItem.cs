using System;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public abstract class GrabableItem : IGrabableItem
    {
        private ISpaceRouteElement location;
        public abstract IGrabableItemFactoryBase FactoryBase { get; }


        public override string ToString()
        {
            return $"{GetType().Name} : {FactoryBase.Name}";
        }

        public ITextureRenderer Renderer
        {
            get { return FactoryBase.Renderer; }
            set { throw new InvalidOperationException("Render of factory is used!"); }
        }

        public ISpaceRouteElement Location
        {
            get { return location; }
            set
            {
                location?.Tile?.OnObjectLeft(this);
                location = value;
                location?.Tile?.OnObjectEntered(this);
            }
        }

        public void SetLocationNoEvents(ISpaceRouteElement tile)
        {
            location = tile;
        }

        public MapDirection MapDirection { get; set; }
        IRenderer IRenderable.Renderer
        {
            get { return Renderer; }
            set { throw new InvalidOperationException("Render of factory is used!"); }
        }
    }
}
