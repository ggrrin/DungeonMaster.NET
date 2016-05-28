using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class TorsoStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static TorsoStorageType Instance { get; } = new TorsoStorageType();

        private TorsoStorageType() { }
    }
}