using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class ChestStorageType : IStorageType
    {
        public int Size { get; } = 8;

        public static ChestStorageType Instance { get; } = new ChestStorageType();

        private ChestStorageType() { }
    }
}