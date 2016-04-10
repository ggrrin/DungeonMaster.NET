namespace DungeonMasterParser.Items
{
    public class ScrollItem : GrabableItem
    {
        public override T GetItem<T>(IItemCreator<T> t)
        {
            return t.GetItem(this);
        }

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //    1 word:
        //    Scroll attributes
        //        Bits 15 - 10: Used at runtime
        //            '0' Open(in hand)
        //            '1' Closed


        //        Bits 9 - 0: Referred text in list of text objects.
        public int ReferredTextIndex { get; set; }


        //Notes:
        //    Scrolls ignore the visibility attribute of the referred text
        //    In some versions like Chaos Strikes Back for Amiga v3.3, if the text is "MAGICMAP" or "CREANAME" then the scroll acts either as the magic map or displays the name of the creature in front of the party.
        //    There are 14 characters maximum on each line, including the end of text character but not the carriage return characters.If the end of text character is the 15th character on a line, then garbage will be displayed after. In some versions, you need at least two lines of text or the program will display an additional line with garbage.

    }
}