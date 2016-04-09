using DungeonMasterParser.Enums;
using DungeonMasterParser.Items.@abstract;

namespace DungeonMasterParser.Items
{
    public class DoorItem : TileObject
    {
        public override T GetItem<T>(IItemCreator<T> t)
        {
            return t.GetItem(this);
        }

        //Door objects must always be created on door tiles.

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //    1 word: Door attributes.
        //        Bits 15-9: Unused.
        //        Bit 8: Destructible by chopping.
        //            '0' Not Destructible
        //            '1' Destructible
        public bool IsChoppingDestructible { get; set; }

        //        Bit 7: Destructible by Fireball.
        //            '0' Not Destructible
        //            '1' Destructible
        public bool IsFireballDestructible { get; set; }

        //        Bit 6: Button.
        //            '0' Door without button
        //            '1' Door with button
        public bool HasButton { get; set; }

        //        Bit 5: Opening direction.
        //            '0' Horizontal
        //            '1' Vertical
        public OpenDirection OpenDirection { get; set; }

        //        Bits 4-1: Ornate number (0 no ornate) Be careful to only use ornate defined in the map definition, otherwise glitched graphics will appear.
        public int? OrnamentationID { get; set; }

        //TODO what does it mean
        //        Bit 0: Door type(defines the appearance based on door graphics declared in map)
        //map.DoorType
        public bool DoorAppearance { get; set; }

        //The attack power required to destroy a door depends on its type.The resistance values are stored in the graphics.dat file in item 559, offset 244h (4 words). Wooden doors are easy, grate doors are harder, iron doors are very hard and Ra doors are nearly indestructible.

    }
}