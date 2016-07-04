using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    struct WoundInfo
    {
        public bool UseSharpDefense { get; }
        public IStorageType WoundIndex { get; }

        public WoundInfo(bool useSharpDefense, IStorageType woundIndex)
        {
            UseSharpDefense = useSharpDefense;
            WoundIndex = woundIndex;
        }
    }
}