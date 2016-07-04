using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.Tiles.Sides
{
    public class FloorSpaceManager
    {
        public IEnumerable<FloorItemStorage> Storages { get; }

        public FloorSpaceManager(IEnumerable<FloorItemStorage> storages)
        {
            Storages = storages;
        }

        public FloorItemStorage GetSpace(IGrabableItem grabable)
        {
            var space = grabable?.Location?.Space;
            if (space != null)
            {
                return Storages.FirstOrDefault(s => s.Space.Area.Intersects(space.Area)) ?? Storages.First();
            }
            else
            {
                return Storages.First();
            }
        }
    }
}