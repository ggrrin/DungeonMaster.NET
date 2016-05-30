using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class TorsoStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static TorsoStorageType Instance { get; } = new TorsoStorageType();

        private TorsoStorageType() { }
    }
}