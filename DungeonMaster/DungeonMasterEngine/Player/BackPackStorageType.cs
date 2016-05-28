using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class BackPackStorageType : IStorageType
    {
        public int Size { get; } = 17;

        public static BackPackStorageType Instance { get; } = new BackPackStorageType();

        private BackPackStorageType() { }
    }
}