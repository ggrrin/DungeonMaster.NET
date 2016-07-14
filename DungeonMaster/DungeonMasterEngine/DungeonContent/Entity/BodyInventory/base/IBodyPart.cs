namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base
{
    public interface IBodyPart : IInventory
    {
        bool IsWound { get; set; }
        int InjuryMultipler { get; }
    }
}