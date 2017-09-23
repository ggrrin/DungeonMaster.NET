using System;
using System.Collections.Generic;
using DungeonMasterParser.Items;

namespace DungeonMasterParser.Tiles
{
    public abstract class TileData 
    {
        //Bit 4: Object(s) on this tile

        //'0' No object on this tile
        //'1' A list of object on this tile
        public bool HasItemsList { get; set; }

        public ObjectID FirstObject { get; set; }

        public List<ActuatorItemData> Actuators { get; } = new List<ActuatorItemData>();

        public List<GrabableItemData> GrabableItems { get; } = new List<GrabableItemData>();

        public List<CreatureItem> Creatures { get; set; } = new List<CreatureItem>();
        public List<TextDataItem> TextTags { get; } = new List<TextDataItem>();

        public abstract T GetTile<T>(ITileCreator<T> t);

        internal IEnumerable<ItemData> GetItems(DungeonData d)
        {
            return GetEnumerator(new ItemEnumerator(d, FirstObject));
        }

        private IEnumerable<ItemData> GetEnumerator(ItemEnumerator d)
        {
            while (d.MoveNext())
                yield return d.Current;
        }

        protected virtual int? GetRandomWallDecoration(DungeonMap CurrentMap, Random rand)
        {
            int val = rand.Next(29);
            return val < CurrentMap.WallDecorationGraphicsCount ? val : (int?)null;
        }

        public abstract void SetupDecorations(DungeonMap map, Random rand);
    }
}