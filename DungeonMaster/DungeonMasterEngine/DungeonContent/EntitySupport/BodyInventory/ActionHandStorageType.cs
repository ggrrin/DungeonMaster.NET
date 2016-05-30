using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    internal class ActionHandStorageType : IStorageType
    {
        public int Size { get; } = 1;

        public static ActionHandStorageType Instance { get; } = new ActionHandStorageType();

        private ActionHandStorageType() { }
    }
}