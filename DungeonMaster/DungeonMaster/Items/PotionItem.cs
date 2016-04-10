namespace DungeonMasterParser.Items
{
    public class PotionItem : GrabableItem
    {
        public override T GetItem<T>(IItemCreator<T> t)
        {
            return t.GetItem(this);
        }

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //    1 word:
        //    Item attributes.
        //        Bits 15: Important item. An important item will not be flushed if the game has no more free space to store items dropped by creatures. Only non important items can be flushed.Items dropped by creatures have this bit set to 0.
        public bool IsImportant { get; set; }
        
        //       Bits 14 - 8: Potion type. You can refer to the Technical Documentation - Dungeon Master and Chaos Strikes Back Items properties page to find the complete list of possible values.
        //public int ItemTypeIndex { get; set; }
        
        //        Bits 7 - 0: Potion power. In Dungeon Master and Chaos Strikes Back, the power displayed to the player is obtained by dividing the power value by 40.The result is the symbol number between 1 and 6.There is a bug in the code that shows an underscore character instead of a power symbol for Potions with power < 40.This bug also applies to 'Empty Flask' which has Power = 0(but not 'Water Flask').
        public int PotionPower { get; set; }
        
        //        In Dungeon Master II, the potion power is displayed to the player with a bar.

    }
}