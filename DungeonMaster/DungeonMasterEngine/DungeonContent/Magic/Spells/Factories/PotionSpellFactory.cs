using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class PotionSpellFactory : SpellFactory<PotionSpell>
    {
        public PotionFactory PotionFactory { get; }
        public IReadOnlyList<PotionFactory> PotionFactories { get; }

        public PotionSpellFactory(SpellFactoryInitializer initializer, PotionFactory potionFactory, IReadOnlyList<PotionFactory> potionFactories) : base(initializer)
        {
            PotionFactory = potionFactory;
            PotionFactories = potionFactories;
        }

        protected override PotionSpell ApplySpellEffect(ILiveEntity entity, IPowerSymbol powerSymbol, int skillLevel)
        {
            return new PotionSpell(powerSymbol, this);
        }
    }
}