namespace DungeonMasterParser.Items
{
    public class WeaponItem : GrabableItem
    {
        public override T GetItem<T>(IItemCreator<T> t)
        {
            return t.GetItem(this);
        }

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }


        //    1 word:
        //    Item description
        //        Bit 15: Used at runtime, for example set when torch is lit


        //        Bits 14: Broken
        public bool IsBroken { get; set; }

        //        Bits 13 - 10: Number of charges.For 'Torch' objects, this represents the Luminous Power.
        public int ChargeCount { get; set; }

        //        Bit 9: Poisoned
        public bool IsPoisoned { get; set; }

        //        Bit 8: Cursed.Falchions dropped by Animated Armours and Deth Knights have this attribute set.
        //            '0' Not cursed
        //            '1' Cursed
        public bool IsCursed { get; set; }

        //        Bits 7: Important item.An important item will not be flushed if the game has no more free space to store items dropped by creatures. Only non important items can be flushed.Items dropped by creatures have this bit set to 0.
        public bool IsImportant {get; set;}
        
        //      Bits 6 - 0: Item type. You can refer to the Technical Documentation - Dungeon Master and Chaos Strikes Back Items properties page to find the complete list of possible values.
        //public int ItemTypeIndex { get; set; }
        

    }
}