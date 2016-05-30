using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class HandStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static HandStorageType Instance { get; } = new HandStorageType();

        private HandStorageType() { }
    }
}