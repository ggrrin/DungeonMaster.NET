using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class LegsStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static LegsStorageType Instance { get; } = new LegsStorageType();

        private LegsStorageType() { }
    }
}