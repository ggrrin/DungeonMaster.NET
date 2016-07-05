namespace DungeonMasterEngine.DungeonContent.Entity.Actions.Projectiles.Explosions
{
    public class PoisonBoltExplosionImpact : ExplosionImpact
    {
        public override bool CreatesExplosion => false;

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