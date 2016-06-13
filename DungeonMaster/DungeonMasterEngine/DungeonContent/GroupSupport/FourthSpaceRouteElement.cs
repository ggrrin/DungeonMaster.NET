using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    class OnethSpaceRouteElement : ISpaceRouteElement
    {
        public Vector3 StayPoint => Tile.Position +  new Vector3(Space.Area.X, 0,  Space.Area.Y) / 1000f + new Vector3(1/2f,0,1/2f);//TODO remove 1/8
        public ISpace Space { get; }
        public ITile Tile { get; }

        public OnethSpaceRouteElement(ISpace space, ITile tile)
        {
            Space = space;
            Tile = tile;
        }
    }
    class FourthSpaceRouteElement : ISpaceRouteElement
    {
        public Vector3 StayPoint => Tile.Position +  new Vector3(Space.Area.X, 0,  Space.Area.Y) / 1000f + new Vector3(1/8f,0,1/8f);//TODO remove 1/8
        public ISpace Space { get; }
        public ITile Tile { get; }

        public FourthSpaceRouteElement(ISpace space, ITile parentTile)
        {
            Space = space;
            Tile = parentTile;
        }
    }
}