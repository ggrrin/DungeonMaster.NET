using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class ChestStorageType : IStorageType
    {
        public int Size { get; } = 8;

        public static ChestStorageType Instance { get; } = new ChestStorageType();

        private ChestStorageType() { }
    }
}