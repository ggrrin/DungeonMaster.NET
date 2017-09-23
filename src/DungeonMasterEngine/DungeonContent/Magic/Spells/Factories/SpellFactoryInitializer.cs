using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class SpellFactoryInitializer
    {
        public IEnumerable<ISpellSymbol> CastingSequence { get; set; }
        public string Name { get; set; }
        public int Difficulty { get; set; }
        public int Duration { get; set; }
        public ISkillFactory Skill { get; set; }
        public int SkillLevel { get; set; }
    }
}