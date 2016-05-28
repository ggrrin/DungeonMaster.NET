using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class HeadStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static HeadStorageType Instance { get; } = new HeadStorageType();

        private HeadStorageType() { }
    }
}