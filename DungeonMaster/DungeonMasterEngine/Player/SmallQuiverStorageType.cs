using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class SmallQuiverStorageType : IStorageType
    {
        public int Size { get; } = 3;
        public static SmallQuiverStorageType Instance { get; } = new SmallQuiverStorageType();

        private SmallQuiverStorageType() { }
    }
}