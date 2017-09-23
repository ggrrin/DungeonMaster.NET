using System.Collections;
using System.Collections.Generic;
using DungeonMasterParser.Items;

namespace DungeonMasterParser.Tiles
{
    public struct ItemEnumerator : IEnumerator<ItemData>
    {
        private DungeonData data;

        private ObjectID nextObjectId, backup;

        public ItemEnumerator(DungeonData d, ObjectID nextObjectId) : this()
        {
            data = d;
            this.nextObjectId = backup = nextObjectId;
        }


        public ItemData Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            data = null;
            nextObjectId = null;
            backup = null;
        }

        public bool MoveNext()
        {
            if (nextObjectId == null || nextObjectId.IsNull)
                return false;

            Current = nextObjectId.GetObject(data);
            Current.ObjectID = nextObjectId;
            nextObjectId = new ObjectID(Current.NextObjectID);
            return true;
        }

        public void Reset()
        {
            nextObjectId = backup;
        }
    }
}