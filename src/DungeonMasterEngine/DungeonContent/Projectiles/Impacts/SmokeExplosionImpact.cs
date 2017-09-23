using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actions;

namespace DungeonMasterEngine.DungeonContent.Projectiles.Impacts
{
    public class SmokeExplosionImpact : ExplosionImpact
    {
        public override bool CreatesExplosion => true;

        protected override AttackInfo GetDefaultImpact(Projectile projectile)
        {
            //TODO what about damage ?? original code in Impact base class
            return new AttackInfo
            {
                Type = CreatureAttackType.C5_ATTACK_MAGIC
            };
        }

        public override async void FinishImpact(Projectile projectile)
        {
            var attack = projectile.KineticEnergy;
            //send event ==>

            while(attack > 55)
            {
                attack -= 40;
                await Task.Delay(1000/6);
            }
        }
    }
}