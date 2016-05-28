using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class NeckStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static NeckStorageType Instance { get; } = new NeckStorageType();

        private NeckStorageType() { }
    }
}