using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class PouchStorageType : IStorageType
    {
        public int Size { get; } = 2;

        public static PouchStorageType Instance { get; } = new PouchStorageType();

        private PouchStorageType() { }
    }
}