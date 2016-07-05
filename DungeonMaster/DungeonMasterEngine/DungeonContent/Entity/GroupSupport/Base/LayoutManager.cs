using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base
{
    public class LayoutManager<TItem> where TItem  : class
    {
        private readonly List<Tuple<TItem, ISpace>> entitiesSpaces = new List<Tuple<TItem, ISpace>>();

        private IEnumerable<ISpace> FullSpaces => entitiesSpaces.Select(x => x.Item2);

        public IEnumerable<TItem> Entities => entitiesSpaces.Select(x => x.Item1);

        public virtual bool IsFree(ISpace space) => FullSpaces.All(s => !s.Area.Intersects(space.Area));

        public bool WholeTileEmpty => !FullSpaces.Any();

        public virtual bool TryGetSpace(TItem entity, ISpace dreamPosition)
        {
            var res = FullSpaces.Any(s => s.Area.Intersects(dreamPosition.Area));
            if (!res)
                entitiesSpaces.Add(Tuple.Create(entity, dreamPosition));
            else
            {
                
            }
            return !res;
        }

        public virtual void FreeSpace(TItem entity, ISpace space)
        {
            int index = entitiesSpaces.FindIndex(t => t.Item1 == entity && t.Item2 == space);
            if(index == -1 )
                throw new InvalidOperationException("Record is not in collection.");
            entitiesSpaces.RemoveAt(index);
        }

        public virtual IEnumerable<TItem> GetEntities(ISpace space)
        {
            return entitiesSpaces
                .Where(t => t.Item2.Area.Intersects(space.Area))
                .Select(t => t.Item1);
        }
    }
}