using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties.Base
{
    public abstract class Property : IProperty
    {
        public event EventHandler<int> ValueChanged;

        protected int value;
        public virtual int MaxValue => BaseValue + AdditionalValues.Sum(x => x.Value); 
        public abstract int BaseValue { get; set; }

        public virtual int Value
        {
            get { return value; }
            set
            {
                var prevVal = this.value;
                this.value = MathHelper.Clamp(value, 0, MaxValue);
                $"{GetType().Name}: {Value} of {MaxValue} ; {this.value - prevVal}".Dump();
                ValueChanged?.Invoke(this, Value);
            }
        }

        public abstract IPropertyFactory Type { get; }
        public ICollection<IEntityPropertyEffect> AdditionalValues { get; } = new HashSet<IEntityPropertyEffect>();
    }
}