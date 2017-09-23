using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class EmptyProperty : IProperty {
        public int MaxValue { get; }
        public int BaseValue { get; set; }
        public int Value { get; set; }
        public IPropertyFactory Type => PropertyFactory<EmptyProperty>.Instance;
        public ICollection<IEntityPropertyEffect> AdditionalValues { get; } = null;
    }
}