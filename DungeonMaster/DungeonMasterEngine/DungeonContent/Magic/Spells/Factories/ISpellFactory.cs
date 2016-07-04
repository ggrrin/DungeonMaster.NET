using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public interface ISpellFactory<out TSpell> where TSpell : ISpell
    {
        IEnumerable<ISpellSymbol> CastingSequence { get; }
        string Name { get; }
        int Difficulty { get; }
        int Duration { get; }
        ISkillFactory Skill{ get; }
        int SkillLevel { get; }

        TSpell CastSpell(IPowerSymbol powerSymbol, ILiveEntity caster);
    }
}