using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine
{
    public class LevelCollection : IEnumerable<DungeonLevel>, IEnumerable, ICollection<DungeonLevel>
    {
        private Queue<DungeonLevel> queue = new Queue<DungeonLevel>();

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<DungeonLevel> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)queue).GetEnumerator();
        }

        public void Add(DungeonLevel item)
        {
            queue.Enqueue(item);

            if (Count > 3)
                queue.Dequeue();
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
