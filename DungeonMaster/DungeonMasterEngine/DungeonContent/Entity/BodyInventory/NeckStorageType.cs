using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory
{
    internal class NeckStorageType : IStorageType
    {
        public bool IsBodyPart { get; } = true;
        public int Size { get; } = 1;

        public static NeckStorageType Instance { get; } = new NeckStorageType();

        private NeckStorageType() { }
    }
}