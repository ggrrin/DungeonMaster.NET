using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class PouchStorageType : IStorageType
    {
        public int Size { get; } = 2;

        public static PouchStorageType Instance { get; } = new PouchStorageType();

        private PouchStorageType() { }
    }
}