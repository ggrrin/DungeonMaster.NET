using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class ConsumableStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static ConsumableStorageType  Instance { get; } = new ConsumableStorageType();

        private ConsumableStorageType() { }
    }
}