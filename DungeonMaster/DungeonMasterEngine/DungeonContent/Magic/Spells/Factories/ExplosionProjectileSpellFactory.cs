using DungeonMasterEngine.Builders;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Projectiles.Impacts;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class ExplosionProjectileSpellFactory<TImpact> : ProjectilSpellFactory<ExplosionProjectilSpell<TImpact>> where TImpact : ExplosionImpact, new()
    {
        public IRenderersSource RenderersSource { get; }
        public Texture2D Texture { get; }

        public ExplosionProjectileSpellFactory(SpellFactoryInitializer initializer, IRenderersSource renderersSource, Texture2D texture) : base(initializer)
        {
            RenderersSource = renderersSource;
            Texture = texture;
        }

        protected override ExplosionProjectilSpell<TImpact> ApplySpellEffect(ILiveEntity entity, IPowerSymbol powerSymbol, int skillLevel)
        {
            int kineticEnergy = MathHelper.Clamp(21, (powerSymbol.LevelOrdinal + 2) * (4 + (skillLevel << 1)), 255);
            var values = F327_kzzz_CHAMPION_IsProjectileSpellCast(entity, kineticEnergy, 0);
            if (values != null)
            {
                var val = values.Value;
                var res = new ExplosionProjectilSpell<TImpact>(val.KineticEnergy, val.StepEnergy, val.Attack);
                res.Projectile.Renderer = RenderersSource.GetProjectileSpellRenderer(res.Projectile, Texture);
                return res;
            }
            else
            {
                return null;
            }
        }

    }
}