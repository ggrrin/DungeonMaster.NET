using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class BigQuiverStorageType : IStorageType
    {
        public bool IsBodyPart { get; } = false;
        public int Size { get; } = 1;
        public static BigQuiverStorageType Instance { get; } = new BigQuiverStorageType();

        private BigQuiverStorageType() { }
    }
}