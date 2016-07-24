namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base
{
    public interface IBodyPart : IInventory
    {
        bool IsWounded { get; set; }
        int InjuryMultipler { get; }
    }
}