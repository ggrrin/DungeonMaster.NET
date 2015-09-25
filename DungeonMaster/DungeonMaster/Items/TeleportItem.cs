namespace DungeonMasterParser
{
    public class TeleporterItem : TileObject
    {
        public override T GetItem<T>(IItemCreator<T> t)
        {
            return t.GetItem(this);
        }

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //    1 word: Teleporter attributes
        //        Bit 15: Sound.
        //            '0' No sound
        //            '1' Sound
        public bool HasSound { get; set; }


        //        Bits 14-13: Scope
        //            '00' Items
        //            '01' Creatures
        //            '10' Items / Party
        //            '11' Everything
        public TeleportScope Scope { get; set; }

        //        Bit 12 Rotation type
        //            '0' Relative rotation based on current party orientation.
        //            '1' Absolute rotation from North.
        public RotationType RotationType { get; set; }

        //        Bits 11-10: Rotation
        //            '00' None/North
        //            '01' 90° clockwise/East
        //            '10' 180°/South
        //            '11' 90° anti-clockwise/West
        public Direction Rotation { get; set; }


        //        Bits 9-5: Destination Y coordinate (without map offset)
        //        Bits 4-0: Destination X coordinate(without map offset)
        public Position DestinationPosition { get; set; }

        //    1 word: Destination map
        //        Bits 15-8: Destination map(the game hangs if you teleport to a non existant map)
        public int MapIndex { get; set; }

        //        Bits 7-0: 00h Unused

    }
}