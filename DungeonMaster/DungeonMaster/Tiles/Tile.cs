using System.Collections;
using System.Collections.Generic;
using DungeonMasterParser.Items;
using DungeonMasterParser.Items.@abstract;

namespace DungeonMasterParser.Tiles
{
    public abstract class Tile 
    {
        //Bit 4: Object(s) on this tile

        //'0' No object on this tile
        //'1' A list of object on this tile
        public bool HasItemsList { get; set; }

        public ObjectID FirstObject { get; set; }

        public List<ActuatorItem> Actuators { get; } = new List<ActuatorItem>();

        public List<SuperItem> Items { get; } = new List<SuperItem>();

 
        public abstract T GetTile<T>(ITileCreator<T> t);

        internal IEnumerable<SuperItem> GetItems(DungeonData d)
        {
            return GetEnumerator(new ItemEnumerator(d, FirstObject));
        }

        private IEnumerable<SuperItem> GetEnumerator(ItemEnumerator d)
        {
            while (d.MoveNext())
                yield return d.Current;
        }

    }
    public struct ItemEnumerator : IEnumerator<SuperItem>
    {
        private DungeonData data;

        private ObjectID nextObjectId, backup;

        public ItemEnumerator(DungeonData d, ObjectID nextObjectId) : this()
        {
            data = d;
            this.nextObjectId = backup = nextObjectId;
        }


        public SuperItem Current { get; private set; }

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