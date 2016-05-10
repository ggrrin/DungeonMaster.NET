using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class LayoutManager
    {
        private readonly List<Tuple<ILayoutable, ISpace>> entitiesSpaces = new List<Tuple<ILayoutable, ISpace>>();

        private IEnumerable<ISpace> FullSpaces => entitiesSpaces.Select(x => x.Item2);

        public IEnumerable<ILayoutable> Entities => entitiesSpaces.Select(x => x.Item1);

        public ISpace FindCurrentSpace(ILayoutable enitiy) => entitiesSpaces.Find(t => t.Item1 == enitiy).Item2;

        public bool IsFree(ISpace space) => FullSpaces.All(s => !s.Area.Intersects(space.Area));

        public bool WholeTileEmpty => !FullSpaces.Any();

        public bool TryGetSpace(ILayoutable entity, ISpace dreamPosition)
        {
            var res = FullSpaces.Any(s => s.Area.Intersects(dreamPosition.Area));
            if (!res)
                entitiesSpaces.Add(Tuple.Create(entity, dreamPosition));
            return !res;
        }

        public void FreeSpace(ILayoutable entity, ISpace space)
        {
            int index = entitiesSpaces.FindIndex(t => t.Item1 == entity && t.Item2 == space);
            if(index == -1 )
                throw new InvalidOperationException("Record is not in collection.");
            entitiesSpaces.RemoveAt(index);
        }

        public IEnumerable<ILayoutable> GetEntities(ISpace space)
        {
            return entitiesSpaces
                .Where(t => t.Item2.Area.Intersects(space.Area))
                .Select(t => t.Item1);
        }

    }
}