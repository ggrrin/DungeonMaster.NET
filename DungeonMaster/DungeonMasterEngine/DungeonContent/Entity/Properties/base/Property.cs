using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties.@base
{
    public abstract class Property : IProperty
    {
        protected int value;
        public virtual int MaxValue => BaseValue + AdditionalValues.Sum(x => x.Value); 
        public abstract int BaseValue { get; set; }

        public virtual int Value
        {
            get { return value; }
            set
            {
                this.value = MathHelper.Clamp(value, 0, MaxValue);
            }
        }

        public abstract IPropertyFactory Type { get; }
        public ICollection<IEntityPropertyEffect> AdditionalValues { get; } = new HashSet<IEntityPropertyEffect>();
    }
}