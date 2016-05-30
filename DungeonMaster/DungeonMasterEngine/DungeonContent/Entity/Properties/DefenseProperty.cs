using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class DefenseProperty : IProperty {
        public int MaxValue { get; }
        public int BaseValue { get; set; }
        public int Value { get; set; }
        public IPropertyFactory Type => PropertyFactory<EmptyProperty>.Instance;
        public ICollection<IEntityPropertyEffect> AdditionalValues { get; }
    }
}