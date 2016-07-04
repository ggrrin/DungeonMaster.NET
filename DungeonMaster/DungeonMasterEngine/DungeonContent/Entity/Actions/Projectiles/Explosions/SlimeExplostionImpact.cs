namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public class SlimeExplostionImpact : ExplosionImpact
    {
        public override bool CreatesExplosion => false; 

        protected override AttackInfo GetDefaultImpact(Projectile projectile)
        {
            int attack = rand.Next(16);
            attack += rand.Next(32);
            return new AttackInfo
            {
                Attack = attack,
                PoisonAttack = attack + 10,
                Type = CreatureAttackType.C3_ATTACK_BLUNT
            };
        }
    }
}