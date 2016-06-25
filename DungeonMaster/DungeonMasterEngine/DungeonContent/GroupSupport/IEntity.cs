using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface IEntity
    {
        IGroupLayout GroupLayout { get; }
        IProperty GetProperty(IPropertyFactory propertyType);
    }

}