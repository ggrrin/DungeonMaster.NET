using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties.Base
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