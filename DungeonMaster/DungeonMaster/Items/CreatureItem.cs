using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterParser.Enums;

namespace DungeonMasterParser.Items
{
    public class CreatureItem : ItemData
    {
        public override T CreateItem<T>(IItemCreator<T> t)
        {
            return t.CreateCreature(this);
        }

        //    00h(0) 1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        //    02h(2) 1 word: Next possession object ID. Although not recommended, it is possible to put a creature as a possession of another creature. When the creature dies, the other one is released.
        public int NextPossessionObjectID { get; set; }

        //    04h(4) 1 byte: Creature type. Here are the possible values in Dungeon Master and Chaos Strikes Back:
        public CreatureType Type { get; set; }


        //    05h(5) 1 byte: Position of each creature on the tile.FFh is a special value when there is a single creature in the center.
        //        Bits 7 - 6: Position of creature 4
        public TilePosition Creature4Position { get; set; }

        //        Bits 5 - 4: Position of creature 3
        public TilePosition Creature3Position { get; set; }

        //        Bits 3 - 2: Position of creature 2
        public TilePosition Creature2Position { get; set; }

        //        Bits 1 - 0: Position of creature 1
        public TilePosition Creature1Position { get; set; }

        //    06h(6) 1 word: Hit points of creature 1
        public int HitPointsCreature1 { get; set; }

        //    08h(8) 1 word: Hit points of creature 2
        public int HitPointsCreature2 { get; set; }

        //    0Ah(10) 1 word: Hit points of creature 3
        public int HitPointsCreature3 { get; set; }

        //    0Ch(12) 1 word: Hit points of creature 4
        public int HitPointsCreature4 { get; set; }

        //    0Eh(14) 1 word:
        //    Bits 15 - 11: Unused
        //      Bits 10: Important object, non flushable
        public bool IsImportant { get; set; }

        //      Bits 9 - 8: Direction
        //            '00' North
        //            '01' East
        //            '10' South
        //            '11' West
        public Direction Direction { get; set; }

        //        Bit 7: Unused

        //        Bits 6 - 5: Number of creatures in the group -1
        public int CreaturesCount { get; set; }

        //        Bit 4: Unused

        //        Bits 3 - 0: Used only during the game

        //  Notes:

        //    There is a bug at least Dungeon Master for Atari ST v1.2 where you should not use creatures 25 and 26: they can cause graphical glitches and crashes.
        //    If a creature is teleported to a map where this type of creature is not allowed, the creature is automatically killed.However, this will cause an error if the creature has possessions(this was fixed in CSBwin).
        //    In Dungeon Master, the direction of each creature can be different. The program manages this at runtime only and the values stored in the dungeon file are ignored.
        //    In Dungeon Master II, items like trees and tables are considered as creatures.
        //    In order to correctly display Black Flames, it is required to add the adequate circular floor decoration.
        public IEnumerable<Tuple<TilePosition, int>> GetCreaturesInfo() => GetAllCreaturesInfo().Take(CreaturesCount + 1);

        private IEnumerable<Tuple<TilePosition, int>> GetAllCreaturesInfo()
        {
            yield return Tuple.Create(Creature1Position, HitPointsCreature1);
            yield return Tuple.Create(Creature2Position, HitPointsCreature2);
            yield return Tuple.Create(Creature3Position, HitPointsCreature3);
            yield return Tuple.Create(Creature4Position, HitPointsCreature4);

        }


    }
}