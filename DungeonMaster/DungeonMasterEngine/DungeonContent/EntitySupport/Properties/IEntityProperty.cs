using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Properties
{
    public interface IEntityProperty
    {
        int CurrentValue { get; }
        int BaseValue { get; set; }

        int Maxiumum { get; set; } //TODO bound current value by min and max
        int Minimum { get; }

        int StoredValue { get; set; }

        IEntityPropertyFactory Type { get; }

        IEnumerable<IEntityPropertyEffect> AdditionalValues { get; }
    }
}