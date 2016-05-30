using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class FeetsStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static FeetsStorageType Instance { get; } = new FeetsStorageType();

        private FeetsStorageType() { }
    }
}