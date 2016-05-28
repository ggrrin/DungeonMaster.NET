using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class HandStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static HandStorageType Instance { get; } = new HandStorageType();

        private HandStorageType() { }
    }
}