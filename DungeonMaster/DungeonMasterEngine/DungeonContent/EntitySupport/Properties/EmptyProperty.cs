using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties.@base;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Properties
{
    public class EmptyProperty : IProperty {
        public int MaxValue { get; }
        public int BaseValue { get; set; }
        public int Value { get; set; }
        public IPropertyFactory Type { get; }
        public ICollection<IEntityPropertyEffect> AdditionalValues { get; } = null;
    }
}