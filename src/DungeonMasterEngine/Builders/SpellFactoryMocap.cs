using System;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;

namespace DungeonMasterEngine.Builders
{
    public class SpellFactoryMocap : SpellFactory<ISpell>
    {
        protected override ISpell ApplySpellEffect(ILiveEntity entity, IPowerSymbol powerSymbol, int skillLevel)
        {
            throw new NotImplementedException();
        }

        public SpellFactoryMocap(SpellFactoryInitializer initializer) : base(initializer) {}
    }
}