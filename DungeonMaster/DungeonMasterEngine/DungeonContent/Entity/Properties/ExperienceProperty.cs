using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class ExperienceProperty : IProperty
    {
        public ExperienceProperty(int experienceGain)
        {
            MaxValue = experienceGain;
        }

        public int MaxValue { get; }
        public int BaseValue
        {
            get { return MaxValue; }
            set { throw new NotImplementedException(); }
        }

        public int Value
        {
            get { return MaxValue; }
            set { throw new NotImplementedException(); }
        }

        public IPropertyFactory Type => PropertyFactory<ExperienceProperty>.Instance;
        public ICollection<IEntityPropertyEffect> AdditionalValues => null;
    }
}