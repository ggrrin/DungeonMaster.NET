using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public interface IEntity
    {
        IProperty GetProperty(IPropertyFactory propertyType);
    }

}