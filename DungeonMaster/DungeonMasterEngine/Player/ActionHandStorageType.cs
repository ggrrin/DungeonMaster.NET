using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    internal class ActionHandStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static ActionHandStorageType Instance { get; } = new ActionHandStorageType();

        private ActionHandStorageType() { }
    }
}