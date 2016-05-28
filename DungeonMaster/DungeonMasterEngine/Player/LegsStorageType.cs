using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class LegsStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static LegsStorageType Instance { get; } = new LegsStorageType();

        private LegsStorageType() { }
    }
}