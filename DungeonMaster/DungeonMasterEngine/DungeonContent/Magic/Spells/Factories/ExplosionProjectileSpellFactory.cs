using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Actions;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class ExplosionProjectileSpellFactory<TImpact> : ProjectilSpellFactory<ExplosionProjectilSpell<TImpact>> where TImpact : ExplosionImpact, new()
    {
        public ExplosionProjectileSpellFactory(SpellFactoryInitializer initializer) : base(initializer) { }

        protected override ExplosionProjectilSpell<TImpact> ApplySpellEffect(ILiveEntity l1270PsChampion, IPowerSymbol l1268IPowerSymbolOrdinal, int a1267UiSkillLevel)
        {
            int kineticEnergy = MathHelper.Clamp(21, (l1268IPowerSymbolOrdinal.LevelOrdinal + 2) * (4 + (a1267UiSkillLevel << 1)), 255);
            var values = F327_kzzz_CHAMPION_IsProjectileSpellCast(l1270PsChampion, kineticEnergy, 0);
            if (values != null)
            {
                var val = values.Value;
                return new ExplosionProjectilSpell<TImpact>(val.KineticEnergy, val.StepEnergy, val.Attack);
            }
            else
            {
                return null;
            }
        }

    }
}