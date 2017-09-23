namespace DungeonMasterParser.Items
{
    public class TextDataItem : ItemData 
    {
        public override T CreateItem<T>(IItemCreator<T> t)
        {
            return t.CreateTextData(this);
        }

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //    1 word: Referred text in text data.
        //        Bits 15-3: If Bit 2 = 0 and Bit 1 = 0, Offset in text data of beginning of text in number  of words. Other cases are not decoded yet.
        //    number of bytes!!
        public int ReferredTextOffset { get; set; }

        //        Bit 2: Only used in Dungeon Master II
        //            '0' Regular text
        //            '1' Text requiring translation

        //        Bit 1: Only used in Dungeon Master II
        //            '0' Regular text
        //            '1' Text is actually a simplified Actuator

        //        Bit 0: Text visibility
        //            '0' Invisible text
        //            '1' Visible text
        public bool IsVisible { get; set; }


        public string Text { get; set; }


        public bool HasTargetingActuator { get; set; } = false;

        public override string ToString()
        {
            return Text;
        }
    }
}