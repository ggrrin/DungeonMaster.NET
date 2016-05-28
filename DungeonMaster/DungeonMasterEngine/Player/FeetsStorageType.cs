using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class FeetsStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static FeetsStorageType Instance { get; } = new FeetsStorageType();

        private FeetsStorageType() { }
    }
}