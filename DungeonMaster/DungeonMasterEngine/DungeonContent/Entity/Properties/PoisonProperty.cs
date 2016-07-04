using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class PoisonProperty : IProperty
    {
        private int value;
        public int MaxValue => int.MaxValue;
        public int BaseValue
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Value
        {
            get { return value; }
            set { this.value = MathHelper.Clamp(value, 0, int.MaxValue); }
        }

        public IPropertyFactory Type => PropertyFactory<PoisonProperty>.Instance;
        public ICollection<IEntityPropertyEffect> AdditionalValues { get; } =  new HashSet<IEntityPropertyEffect>();
    }
}