using DungeonMasterEngine.DungeonContent.Actions;

namespace DungeonMasterEngine.DungeonContent.Projectiles.Impacts
{
    public class PoisonBoltExplosionImpact : ExplosionImpact
    {
        public override bool CreatesExplosion => false;

        public override void FinishImpact(Projectile projectile) { }

        protected override AttackInfo GetDefaultImpact(Projectile projectile)
        {
            return new AttackInfo
            {
                Attack = 1,
                PoisonAttack = projectile.KineticEnergy,
                Type = CreatureAttackType.C5_ATTACK_MAGIC
            };
        }
    }
}