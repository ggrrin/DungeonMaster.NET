using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public interface IChampionInitializator
    {
        IEnumerable<IProperty> GetProperties(Champion champion);
        IEnumerable<ISkill> GetSkills(Champion champion);
    }
}