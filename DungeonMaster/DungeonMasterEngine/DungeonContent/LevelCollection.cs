using System;
using System.Collections;
using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent
{
    public class LevelCollection : IEnumerable<DungeonLevel>, IEnumerable, ICollection<DungeonLevel>
    {
        private Queue<DungeonLevel> queue = new Queue<DungeonLevel>();

        public int Count => queue.Count;

        public bool IsReadOnly => false;

        public IEnumerator<DungeonLevel> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)queue).GetEnumerator();

        public void Add(DungeonLevel item)
        {
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

        public void Clear()
        {
            queue.Clear();
        }

        public bool Contains(DungeonLevel item)
        {
            return queue.Contains(item);            
        }

        public bool Contains(int levelIndex, out DungeonLevel level)
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

        public bool Remove(DungeonLevel item)
        {
            throw new InvalidOperationException("Operiation not supported");
        }

        
    }
}
