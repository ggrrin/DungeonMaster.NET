using DungeonMasterEngine.DungeonContent.Projectiles;
using DungeonMasterEngine.DungeonContent.Projectiles.Impacts;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public class ExplosionProjectilSpell<TImpact> : ProjectileSpell where TImpact : Impact, new()
    {
        public override Projectile Projectile { get; protected set; }
        public ExplosionProjectilSpell(int p681IKineticEnergy, int l0991_i_StepEnergy, int attack) 
        {
            Projectile = new ImpactProjectile(p681IKineticEnergy, l0991_i_StepEnergy, attack, new TImpact());
        }

        //projectile created explicitly instead
        //void F326_ozzz_CHAMPION_ShootProjectile(Champion P674_ps_Champion, object P675_T_Thing, int P676_i_KineticEnergy, int P677_i_Attack, int P678_i_StepEnergy)
    }
}