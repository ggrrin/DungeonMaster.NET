using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class SmallQuiverStorageType : IStorageType
    {
        public int Size { get; } = 3;
        public static SmallQuiverStorageType Instance { get; } = new SmallQuiverStorageType();

        private SmallQuiverStorageType() { }
    }
}