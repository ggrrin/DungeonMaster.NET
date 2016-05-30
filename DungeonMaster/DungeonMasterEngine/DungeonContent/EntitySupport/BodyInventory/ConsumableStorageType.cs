using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class ConsumableStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static ConsumableStorageType  Instance { get; } = new ConsumableStorageType();

        private ConsumableStorageType() { }
    }
}