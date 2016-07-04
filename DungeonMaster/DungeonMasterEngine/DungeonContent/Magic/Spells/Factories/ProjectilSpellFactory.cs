using System.CodeDom;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Actions;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public abstract class ProjectilSpellFactory<TProjectileSpell> : SpellFactory<TProjectileSpell> where TProjectileSpell : ProjectileSpell
    {
        public ProjectilSpellFactory(SpellFactoryInitializer initializer) : base(initializer) {}


        protected ProjectileProperties? F327_kzzz_CHAMPION_IsProjectileSpellCast(ILiveEntity L0992_ps_Champion, int P681_i_KineticEnergy, int P682_ui_RequiredManaAmount)
        {
            var mana = L0992_ps_Champion.GetProperty(PropertyFactory<ManaProperty>.Instance);
            if (mana.Value < P682_ui_RequiredManaAmount)
            {
                return null;
            }

            mana.Value -= P682_ui_RequiredManaAmount;


            //M08_SET(L0992_ps_Champion->Attributes, MASK0x0100_STATISTICS);
            int L0991_i_StepEnergy = 10 - MathHelper.Min(8, mana.MaxValue >> 3);
            if (P681_i_KineticEnergy < (L0991_i_StepEnergy << 2))
            {
                P681_i_KineticEnergy += 3;
                L0991_i_StepEnergy--;
            }
            return new ProjectileProperties(P681_i_KineticEnergy, L0991_i_StepEnergy, 90);
        }

    }
}