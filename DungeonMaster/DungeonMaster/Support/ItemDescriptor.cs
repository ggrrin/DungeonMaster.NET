using DungeonMasterParser.Enums;

namespace DungeonMasterParser.Support
{
    public struct ItemDescriptor
    {
        public int TableIndex { get; }
        public int GlobalItemIndex { get; }
        public int FloorGraphicsIndex { get; }
        public int AttackCombo { get; }
        public int CarryLocationsIndex { get; }
        public CarrryLocations CarryLocation { get; }
        public int InCategoryIndex { get; }
        public ObjectCategory Category { get; }
        public string Name { get; }

        public ItemDescriptor(int globalItemIndex, int floorGraphicsIndex, int attackCombo, int carryLocationsIndex, CarrryLocations carryLocation, int tableIndex, int categoryIndex, ObjectCategory category, string name)
        {
            GlobalItemIndex = globalItemIndex;
            FloorGraphicsIndex = floorGraphicsIndex;
            AttackCombo = attackCombo;
            CarryLocationsIndex = carryLocationsIndex;
            CarryLocation = carryLocation;
            TableIndex = tableIndex;
            InCategoryIndex = categoryIndex;
            Category = category;
            Name = name;
        }
    }
}