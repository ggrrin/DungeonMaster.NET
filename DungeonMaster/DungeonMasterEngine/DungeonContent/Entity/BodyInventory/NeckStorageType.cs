using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class NeckStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static NeckStorageType Instance { get; } = new NeckStorageType();

        private NeckStorageType() { }
    }
}