using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class LegsStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static LegsStorageType Instance { get; } = new LegsStorageType();

        private LegsStorageType() { }
    }
}