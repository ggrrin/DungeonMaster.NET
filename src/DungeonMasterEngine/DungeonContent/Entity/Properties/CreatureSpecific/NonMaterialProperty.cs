using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties.CreatureSpecific
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