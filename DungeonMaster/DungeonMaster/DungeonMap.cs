using System.Collections.Generic;
using System.Reflection;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Tiles;

namespace DungeonMasterParser
{
    public class DungeonMap
    {
        //Each map is defined by 16 bytes(14 maps* 16 bytes = 224 bytes)

        //    00h(00) 1 word: Offset of map data in global map data
        public int GlobalDataOffset { get; set; }

        //    02h(02) 1 word: This word is not used by Dungeon Master and Chaos Strikes Back, where its value is always 0000. It is only used in Dungeon Master II.
        //   This word is a bit field defining which components are used on the map:
        //        Bits 15-9: Unused
        //        Bit 8: Door type 1
        //        Bit 7:  Door type 0
        //        Bit 6: Portraits
        //        Bit 5: Blue haze of teleporters
        //        Bit 4: Stairs going down
        //        Bit 3: Stairs going up
        //        Bit 2: Nearly invisible pit.This bit is always set to 0 in the Dungeon Master II dungeon because it does not use this kind of pits.However the pictures do exist and they work fine, even with this bit set to 0)
        //        Bit 1: Pit in the ceiling.Always set to 1 in the Dungeon Master II dungeon, probably because detecting that with the original dungeon editor was too difficult (as it would need to examine other maps)
        //        Bit 0: Normal pit

        //    The exact role of these bits is not clear.
        //    If one bit for doors is set to 0 then the corresponding doors are not displayed and replaced by a grimacing face (the door frame is still displayed, though).
        //    The other bits can be changed from 1 to 0 without any visible impact as things are still displayed correctly.
        //    This word may be used to allocate memory for graphics or for color palette management.What is known is that the values are used by the code.
        //    04h (04) 1 word: 0000 : Unused


        //    06h(06) 1 byte: Map offset x
        public int OffsetX { get; set; }

        //    07h(07) 1 byte: Map offset y
        public int OffsetY { get; set; }
        
        //    08h(08) 1 word: Map size
        //        Bits 15-11: Map height - 1
        public int Height { get; set; }

        //        Bits 10-6: Map width - 1
        public int Width { get; set; }

        //        Bits 5-0: Level number.Several maps can be at the same level in order to produce large levels.This is only used in Dungeon Master II where for example the exterior level (level 6) is made of 26 maps, resulting in a global size of 86x93 tiles.Here are the associations of levels and maps in the Dungeon Master II dungeon:
        //            Level 1: Map 01
        //            Level 2: Maps 02 03
        //            Level 3: Maps 04 05
        //            Level 4: Maps 06 07
        //            Level 5: Maps 08 09
        //            Level 6: Maps 10 11 14 15-19 22-34 37 39 41-43
        //            Level 7: Maps 00 12 20 21 35 36 40
        //            Level 8: Map 13
        //            Level 9: -
        //            Level 10: Map 38

        //    Note: The number of graphics available for random decorations must be smaller than the number of graphics available in the map.The first graphics in the lists of available graphics (defined at the end of each map data) are used for random decorations.
        //   Random decorations are only placed on the sides of 'Wall' and 'Floor' tiles if they allow them (four bits to allow each side of the tile to receive a decoration). The program generates a random number between 0 and 29. If the random number is lesser than the number of graphics available for random decorations, then the decoration is displayed.
        //    0Ah (10) 1 word: Number of graphics
        //        Bits 15-12: Number of floor graphics available for random decorations
        public int FloorDecorationGraphicsCount { get; set; }

        //        Bits 11-8: Number of floor graphics available in the map
        public int FloorGraphicsCount { get; set; }

        //        Bits 7-4: Number of wall graphics available for random decorations
        public int WallDecorationGraphicsCount { get; set; }

        //        Bits 3-0: Number of wall graphics available in the map
        public int WallGraphicsCount { get; set; }

        
        //    0Ch(12) 1 word:
        //        Bits 15-12: Map difficulty.The difficulty (also known as 'depth') is used as an experience multiplier and to determine the initial hit points of generated creatures.Difficulty 0 produces permanent light, as in the Hall of Champions.
        public int Difficulty { get; set; }

        //      Bits 11-8: These bits are not used by the games, but there are some non zero values:
        //        Dungeon Master: Value 4 for map 3, Value 1 for map 5, Value 3 for map 12
        //        Chaos Strikes Back: Value 3 for map 0, Value 1 for map 1, Value 3 for map 2, Value 4 for map 3, Value 2 for map 4, Value 3 for map 5, Value 3 for map 6, Value 3 for map 7, Value 2 for map 8, Value 2 for map 9
        //        Dungeon Master II: Values 1 to 5
        
        //        Bits 7-4: Number of creatures types allowed in map
        public int CreatureTypeCount { get; set; }

        //      Bits 3-0: Number of door decoration graphics (7 max)
        public int DoorDecorationCount { get; set; }

        //from item 559
        
        //03 6E Grate door: Not animated, items can pass through, creatures can see through, Resistance = 6E
        //00 2A Wooden door: Not animated, items cannot pass through, creatures cannot see through, Resistance = 2A
        //00 E6 Iron door: Not animated, items cannot pass through, creatures cannot see through, Resistance = E6
        //05 FF Ra door: Animated, items cannot pass through, creatures can see through, Resistance = FF



        //    0Eh(14) 1 word: Door indices.These indices define both the graphic to use and the door characteristics (used as an index in the 'Door characteristics' in Item 559)
        //        Bits 15-12: Index for door type 1
        public DoorType DoorType { get; set; }
        //            '0000' (0) Grate door
        //            '0001' (1) Wooden door
        //            '0010' (2) Iron door
        //            '0011' (3) Ra door
        //            Other values: Unknown
        
        //        Bits 11-8: Index for door type 0 (Same possible values as door type 1)
        public int DoorType0Index { get; set; }
        
        //        Bits 7-4: Map graphics style.Must be 0h in Dungeon Master and Chaos Strikes Back because there is only one style available(other values cause graphical glitches or crashes). Here are the possible values in Dungeon Master 2:
        //            0: Blue zone
        //            1: Outside
        //            2: Cave
        //            3: Interior
        //            4: Skullkeep roof
        //            5: Mist


        //        Bits 3-0: 00 : Floor and ceiling graphics style.Must be 0h in Dungeon Master and Chaos Strikes Back because there is only one style available(other values cause graphical glitches or crashes). Unused in Dungeon Master II(always 0h).
        
        //------------------------------------------------------------------------------------------
        //MAP_DATA  
        //------------------------------------------------------------------------------------------


        public IList<TileData> Tiles { get; set; }


        public TileData this[int x, int y]
        {
            get
            {
                int relativeX = x - OffsetX;
                int relativeY = y - OffsetY;

                if (relativeX < 0 || relativeX >= Width || relativeY < 0 || relativeY >= Height )
                    return null;                
                
                return Tiles[relativeX * Height + relativeY];
            }
        }


        public byte[] CreaturesDecoration { get; set; }

        public string[] WallDecorations { get; set; }

        public string[] FloorDecorations { get; set; }

        public string[] DoorDecorations { get; set; }
    }
}