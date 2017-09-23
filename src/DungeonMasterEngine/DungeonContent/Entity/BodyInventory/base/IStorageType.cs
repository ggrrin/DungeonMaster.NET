namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base
{
    public interface IStorageType
    {
        bool IsBodyPart { get; }
        int Size { get; }
    }
}