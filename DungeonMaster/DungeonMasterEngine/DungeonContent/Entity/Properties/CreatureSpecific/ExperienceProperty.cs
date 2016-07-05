using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties.CreatureSpecific
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