using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class NeckStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static NeckStorageType Instance { get; } = new NeckStorageType();

        private NeckStorageType() { }
    }
}