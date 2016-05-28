using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class ChestStorageType : IStorageType
    {
        public int Size { get; } = 8;

        public static ChestStorageType Instance { get; } = new ChestStorageType();

        private ChestStorageType() { }
    }
}