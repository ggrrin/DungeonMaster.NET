using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class PotionSpellFactory : SpellFactory<PotionSpell>
    {
        public PotionFactory PotionFactory { get; }
        public PotionSpellFactory(SpellFactoryInitializer initializer, PotionFactory potionFactory) : base(initializer)
        {
            PotionFactory = potionFactory;
        }

        protected override PotionSpell ApplySpellEffect(ILiveEntity l1270PsChampion, IPowerSymbol l1268IPowerSymbolOrdinal, int a1267UiSkillLevel)
        {
            return new PotionSpell(l1268IPowerSymbolOrdinal, this);
        }
    }
}