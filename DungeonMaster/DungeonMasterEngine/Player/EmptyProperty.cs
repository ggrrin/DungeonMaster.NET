using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    public class EmptyProperty : IEntityProperty {
        public int CurrentValue { get; }
        public int BaseValue { get; set; } 
        public int Maxiumum { get; set; }
        public int Minimum { get; }
        public int StoredValue { get; set; }
        public IEntityPropertyFactory Type { get; }
        public IEnumerable<IEntityPropertyEffect> AdditionalValues { get; }
    }
}