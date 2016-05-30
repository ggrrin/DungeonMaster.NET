using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class HandStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static HandStorageType Instance { get; } = new HandStorageType();

        private HandStorageType() { }
    }
}