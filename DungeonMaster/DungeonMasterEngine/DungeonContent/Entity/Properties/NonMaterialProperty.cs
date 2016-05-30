using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class NonMaterialProperty :IProperty {

        public NonMaterialProperty(bool isNonmaterial)
        {
            MaxValue = 1;
            BaseValue = 1;
            if (isNonmaterial)
            {
                Value = 1;
            }
            else
            {
                Value = 0;
            }
        }
        public int MaxValue { get; }
        public int BaseValue { get; set; }
        public int Value { get; set; }
        public IPropertyFactory Type => PropertyFactory<NonMaterialProperty>.Instance;
        public ICollection<IEntityPropertyEffect> AdditionalValues => null;



    }
}