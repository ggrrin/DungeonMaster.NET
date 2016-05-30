using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class BackPackStorageType : IStorageType
    {
        public int Size { get; } = 17;

        public static BackPackStorageType Instance { get; } = new BackPackStorageType();

        private BackPackStorageType() { }
    }
}