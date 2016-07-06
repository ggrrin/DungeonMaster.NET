using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport
{
    class OnethSpaceRouteElement : ISpaceRouteElement
    {
        public Vector3 StayPoint => Tile.Position +  new Vector3(Space.Area.X, 0,  Space.Area.Y) / 1000f + new Vector3(1/2f,0,1/2f);//TODO remove 1/8
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