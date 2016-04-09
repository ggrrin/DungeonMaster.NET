using DungeonMasterParser.Items.@abstract;

namespace DungeonMasterParser.Items
{
    public class ClothItem : GrabableItem
    {
        public override T GetItem<T>(IItemCreator<T> t)
        {
            return t.GetItem(this);
        }

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //    1 word:
        //    Item attributes
        //        Bit 15 - 14: Unused

        //          Bit 13: Broken
        public bool IsBroken { get; set; }

        //          Bit 12 - 9: Number of charges. This value is not used.There is a function in the game to decrement the number of charges of an object and it would decrement this value when called for an object of type 'Clothe' however this never happens as all actions causing a decrement of the number of charges apply only to object of type 'Weapon'.
        
        //          Bit 8: Cursed. Torso plate, Armet, Foot plate and Leg plate dropped by Animated Armours and Deth Knights have this attribute set.
        //              '0' Not cursed
        //              '1' Cursed
        public bool IsCursed { get; set; }

        //          Bit 7: Important item. An important item will not be flushed if the game has no more free space to store items dropped by creatures. Only non important items can be flushed.Items dropped by creatures have this bit set to 0.
        public bool IsImportant { get; set; }

        //        Bits 6 - 0: Item type. You can refer to the Technical Documentation - Dungeon Master and Chaos Strikes Back Items properties page to find the complete list of possible values.
        //public int ItemTypeIndex { get; set; }
    }
}