namespace DungeonMasterParser
{
    public class ObjectID
    {
        //Object ID
        public ushort ID { get; set; }

        //    1 word:
        //        Bits 15-14: Object position on tile: North/Top left (00) East/Top right(01) South/Bottom left(10) West/bottom right(11)
        public TilePosition TilePosition { get; set; }

        //        Bits 13-10: Object category
        public ObjectCategory Category { get; set; }


        //      Bits 9-0: Object number in list. Valid values range from 0 to 1021.
        public int ObjectNumber { get; set; }


        
        public SuperItem GetObject(DungeonData d)
        {
            if (IsNull)
                return null;

            switch (Category)
            {
                case ObjectCategory.Actuators:
                    return d.Actuators[ObjectNumber];

                case ObjectCategory.Clothe:
                    return d.Clothes[ObjectNumber];

                case ObjectCategory.Container:
                    return d.Containers[ObjectNumber];

                case ObjectCategory.Creatures:
                    return d.Creatures[ObjectNumber];

                case ObjectCategory.Doors:
                    return d.Doors[ObjectNumber];

                case ObjectCategory.Miscelaneous:
                    return d.MiscellaneousItems[ObjectNumber];

                case ObjectCategory.Potion:
                    return d.Potions[ObjectNumber];

                case ObjectCategory.Scroll:
                    return d.Scrolls[ObjectNumber];

                case ObjectCategory.Teleporters:
                    return d.Teleports[ObjectNumber];

                case ObjectCategory.WallTextsAndMessages:
                    return d.Texts[ObjectNumber];

                case ObjectCategory.Weapon:
                    return d.Weapons[ObjectNumber];

                default:
                    return null;
            }
        }

        public bool IsNull { get; set; }

     

        public ObjectID(ushort data)
        {
            if (data == 0xFFFE)
                IsNull = true;
            else
            {
                ID = data;
                TilePosition = (TilePosition)((data >> 14) & DungeonParser.twoBitsMask);
                Category = (ObjectCategory)((data >> 10) & DungeonParser.fourBitsMask);
                ObjectNumber = data & DungeonParser.tenBitsMask;
            }
        }

        public override string ToString()
        {
            return string.Format("ID={0}; TilePosition={1}; Category={2}; ObjectNumber={3};", ID, TilePosition, Category, ObjectNumber);
        }
    }
}