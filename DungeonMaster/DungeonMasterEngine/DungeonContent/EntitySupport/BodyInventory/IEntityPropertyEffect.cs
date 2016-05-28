using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory
{
    public interface IEntityPropertyEffect
    {
        IEntityPropertyFactory AffectedEntityProperty { get; }
    }
}