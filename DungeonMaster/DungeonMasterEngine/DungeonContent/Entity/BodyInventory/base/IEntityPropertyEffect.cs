using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base
{
    public interface IEntityPropertyEffect
    {
        int Value { get; }
        IPropertyFactory AffectedProperty { get; }
    }
}