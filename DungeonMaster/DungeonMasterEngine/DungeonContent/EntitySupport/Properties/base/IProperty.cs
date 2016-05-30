using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Properties.@base
{
    public interface IProperty
    {
        int MaxValue { get; }
        int BaseValue { get; set; }

        int Value { get; set; }

        IPropertyFactory Type { get; }

        ICollection<IEntityPropertyEffect> AdditionalValues { get; }
    }
}