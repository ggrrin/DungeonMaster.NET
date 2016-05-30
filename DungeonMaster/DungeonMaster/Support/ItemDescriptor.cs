using DungeonMasterParser.Enums;

namespace DungeonMasterParser.Support
{
    public struct ItemDescriptor
    {
        public int TableIndex { get; set; }
        public int GlobalItemIndex { get; set; }
        public int FloorGraphicsIndex { get; set; }
        public int AttackCombo { get; set; }
        public int CarryLocationsIndex { get; set; }
        public CarrryLocations CarryLocation { get; set; }
        public int InCategoryIndex { get; set; }
        public ObjectCategory Category { get; set; }
        public string Name { get; set; }

        //public ItemDescriptor(int globalItemIndex, int floorGraphicsIndex, int attackCombo, int carryLocationsIndex, CarrryLocations carryLocation, int tableIndex, int categoryIndex, ObjectCategory category, string name)
        //{
        //    GlobalItemIndex = globalItemIndex;
        //    FloorGraphicsIndex = floorGraphicsIndex;
        //    AttackCombo = attackCombo;
        //    CarryLocationsIndex = carryLocationsIndex;
        //    CarryLocation = carryLocation;
        //    TableIndex = tableIndex;
        //    InCategoryIndex = categoryIndex;
        //    Category = category;
        //    Name = name;
        //}
    }
}