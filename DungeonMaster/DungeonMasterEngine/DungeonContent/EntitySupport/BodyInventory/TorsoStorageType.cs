using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class TorsoStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static TorsoStorageType Instance { get; } = new TorsoStorageType();

        private TorsoStorageType() { }
    }
}