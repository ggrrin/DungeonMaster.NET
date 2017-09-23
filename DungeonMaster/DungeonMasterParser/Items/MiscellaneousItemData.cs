namespace DungeonMasterParser.Items
{
    public class MiscellaneousItemData : GrabableItemData
    {
        public override T CreateItem<T>(IItemCreator<T> t)
        {
            return t.CreateMisc(this);
        }

        //     1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //     1 word: Item attributes
        //         Bits 15 - 14: Used for Waterskin / Compass / Bones
        //               '00' Empty / North / Champion #1
        //            '01' Almost empty / East / Champion #2
        //            '10' Almost full / South / Champion #3
        //            '11' Full / West / Champion #4
        public int AttributeValueIndex { get; set; }


        //        Also used by Illumulet & Jewel Symal(worn / not worn).
        //           There is a function in the game to decrement the number of charges of an object and it would decrement this value when called for an object of type 'Miscellaneous object' however this never happens as all actions causing a decrement of the number of charges apply only to object of type 'Weapon'.
        //           Bits 13 - 8: Unused

        //           Bits 7: Important item.An important item will not be flushed if the game has no more free space to store items dropped by creatures. Only non important items can be flushed.Items dropped by creatures have this bit set to 0.
        public bool IsImportant { get; set; }
        
        //         Bits 6 - 0: Item type. You can refer to the Technical Documentation - Dungeon Master and Chaos Strikes Back Items properties page to find the complete list of possible values.
        //public int ItemTypeIndex { get; set; }



    }
}