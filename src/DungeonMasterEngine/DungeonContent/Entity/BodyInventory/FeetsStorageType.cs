using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class FeetsStorageType : IStorageType
    {
        public bool IsBodyPart { get; } = true;
        public int Size { get; } = 1;

        public static FeetsStorageType Instance { get; } = new FeetsStorageType();

        private FeetsStorageType() { }
    }
}