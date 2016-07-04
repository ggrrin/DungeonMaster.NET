using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class TorsoStorageType : IStorageType
    {
        public bool IsBodyPart { get; } = true;
        public int Size { get; } = 1;

        public static TorsoStorageType Instance { get; } = new TorsoStorageType();

        private TorsoStorageType() { }
    }
}