using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport
{
    class OnethSpaceRouteElement : ISpaceRouteElement
    {
        public Vector3 StayPoint => Tile.StayPoint;
        public ISpace Space { get; }
        public ITile Tile { get; }

        public ISpaceRouteElement GetNew(ITile tile)
        {
            return new OnethSpaceRouteElement(Space, tile);
        }

        public OnethSpaceRouteElement(ISpace space, ITile tile)
        {
            Space = space;
            Tile = tile;
        }
    }
}