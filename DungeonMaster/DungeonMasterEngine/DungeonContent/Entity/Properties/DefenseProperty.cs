using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class DefenseProperty : Property {
        public DefenseProperty(int value)
        {
            BaseValue = this.value = value;
        }
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<DefenseProperty>.Instance;
    }


    public class ChampionDefenseProperty : DefenseProperty
    {
        private readonly ILiveEntity entity;

        public ChampionDefenseProperty(ILiveEntity entity, int value) : base(value)
        {
            this.entity = entity;
        }
    }


}