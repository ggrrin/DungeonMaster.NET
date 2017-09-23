namespace DungeonMasterParser.Enums
{
    public enum ObjectCategory
    {
        //0000 (0) Doors
        //0001 (1) Teleporters
        //0010 (2) Wall texts and messages
        //0011 (3) Actuators
        //0100 (4) Creatures
        //0101 (5) Weapons
        //0110 (6) Clothes
        //0111 (7) Scrolls
        //1000 (8) Potions
        //1001 (9) Containers
        //1010 (10) Miscellaneous objects
        //1011 (11) Unused
        //1100 (12) Unused
        //1101 (13) Unused
        //1110 (14) Missiles.There must not be such objects in a dungeon file, only in saved games.Otherwise they would stay in place in the dungeon because the associated timer would be missing.
        //1111 (15) Clouds.There must not be such objects in a dungeon file, only in saved games.Otherwise they would stay in place in the dungeon because the associated timer would be missing.

        Doors = 0,
        Teleporters,
        WallTextsAndMessages,
        Actuators,
        Creatures,
        Weapon,
        Clothe,
        Scroll,
        Potion,
        Container,
        Miscellenaous
    }
}