using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class SmallQuiverStorageType : IStorageType
    {
        public int Size { get; } = 3;
        public static SmallQuiverStorageType Instance { get; } = new SmallQuiverStorageType();

        private SmallQuiverStorageType() { }
    }
}