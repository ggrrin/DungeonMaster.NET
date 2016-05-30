using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base
{
    public interface IEntityPropertyEffect
    {
        int Value { get; }
        IPropertyFactory AffectedProperty { get; }
    }
}