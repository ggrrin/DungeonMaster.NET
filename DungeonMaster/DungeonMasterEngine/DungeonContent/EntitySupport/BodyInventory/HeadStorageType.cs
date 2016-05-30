using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class HeadStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static HeadStorageType Instance { get; } = new HeadStorageType();

        private HeadStorageType() { }
    }
}