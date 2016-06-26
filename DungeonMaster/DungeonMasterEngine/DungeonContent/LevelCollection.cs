using System;
using System.Collections;
using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent
{
    public class LevelCollection : ICollection<DungeonLevel>
    {
        protected readonly Queue<DungeonLevel> queue = new Queue<DungeonLevel>();

        public DungeonLevel LastAddedLevel { get; protected set; }

        public int Count => queue.Count;

        public bool IsReadOnly => false;

        public IEnumerator<DungeonLevel> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)queue).GetEnumerator();

        public virtual void Add(DungeonLevel item)
        {
            LastAddedLevel = item;
            queue.Enqueue(item);
            foreach (var creature in item.Creatures)
            {
                creature.Activated = true;
            }

            if (Count > 3)
            {
                foreach (var creature in queue.Dequeue().Creatures)
                {
                    creature.Activated = false;
                }
            }
        }

        public virtual void Clear()
        {
            queue.Clear();
        }

        public virtual bool Contains(DungeonLevel item)
        {
            return queue.Contains(item);            
        }

        public virtual  bool Contains(int levelIndex, out DungeonLevel level)
        {
            foreach (var l in queue)
                if (l.LevelIndex == levelIndex)
                {
                    level = l;
                    return true;
                }

            level = null;
            return false;
        }

        public void CopyTo(DungeonLevel[] array, int arrayIndex)
        {
            queue.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(DungeonLevel item)
        {
            throw new InvalidOperationException("Operiation not supported");
        }

        
    }
}
