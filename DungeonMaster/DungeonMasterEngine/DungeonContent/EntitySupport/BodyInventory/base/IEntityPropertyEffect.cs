using DungeonMasterEngine.DungeonContent.EntitySupport.Properties.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base
{
    public interface IEntityPropertyEffect
    {
        int Value { get; }
        IPropertyFactory AffectedProperty { get; }
    }
}