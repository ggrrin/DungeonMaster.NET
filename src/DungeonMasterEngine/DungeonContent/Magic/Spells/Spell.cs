using System;
using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public abstract class Spell : ISpell
    {
        protected static readonly Random rand = new Random();

        public abstract void Run(ILiveEntity caster, MapDirection direction);
    }
}