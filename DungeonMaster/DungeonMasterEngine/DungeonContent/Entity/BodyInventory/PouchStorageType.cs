using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class PouchStorageType : IStorageType
    {
        public int Size { get; } = 2;

        public static PouchStorageType Instance { get; } = new PouchStorageType();

        private PouchStorageType() { }
    }
}