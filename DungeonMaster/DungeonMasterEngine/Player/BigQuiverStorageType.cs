using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class BigQuiverStorageType : IStorageType
    {
        public int Size { get; } = 1;
        public static BigQuiverStorageType Instance { get; } = new BigQuiverStorageType();

        private BigQuiverStorageType() { }
    }
}