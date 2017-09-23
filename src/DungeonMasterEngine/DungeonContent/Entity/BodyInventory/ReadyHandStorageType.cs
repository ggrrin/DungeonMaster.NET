using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class ReadyHandStorageType : IStorageType
    {
        public bool IsBodyPart { get; } = true;
        public int Size { get; } = 1;

        public static ReadyHandStorageType Instance { get; } = new ReadyHandStorageType();

        private ReadyHandStorageType() { }
    }
}