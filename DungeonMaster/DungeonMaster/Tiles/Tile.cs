using System.Collections.Generic;
using DungeonMasterParser.Items;

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
}