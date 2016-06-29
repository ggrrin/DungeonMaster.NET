using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public interface IChampionInitializer
    {
        IEnumerable<IProperty> GetProperties(Champion champion);
        IEnumerable<ISkill> GetSkills(Champion champion);
    }
}