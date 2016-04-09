using System.Collections.Generic;
using DungeonMasterParser.Items.@abstract;
using DungeonMasterParser.Tiles;

namespace DungeonMasterParser.Items
{
    public class ContainerItem : GrabableItem
    {
        public override T GetItem<T>(IItemCreator<T> t)
        {
            return t.GetItem(this);
        }

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //    1 word: Next contained object ID
        public ObjectID NextContainedObjectID { get; set; }

        //    1 word:
        //        Bits 15 - 3: Unknown(Seems to have random content)
        //        Bits 2 - 1: Container type. In Dungeon Master and Chaos Strikes Back, the only valid value is 00 for the Chests.
        //        Bit 0: Unknown(Seems to have random content)
        //     1 word: 00 00 : Unused


        public IEnumerable<SuperItem> GetEnumerator(DungeonData dungeon)
        {
            var it = new ItemEnumerator(dungeon, NextContainedObjectID);
            while (it.MoveNext())
                yield return it.Current;
        }
    }
}