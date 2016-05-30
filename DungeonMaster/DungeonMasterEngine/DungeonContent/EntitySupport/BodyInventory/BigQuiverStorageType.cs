using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class BigQuiverStorageType : IStorageType
    {
        public int Size { get; } = 1;
        public static BigQuiverStorageType Instance { get; } = new BigQuiverStorageType();

        private BigQuiverStorageType() { }
    }
}