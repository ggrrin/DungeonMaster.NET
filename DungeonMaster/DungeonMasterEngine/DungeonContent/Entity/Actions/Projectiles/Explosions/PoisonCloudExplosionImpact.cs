using System;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public class PoisonCloudExplosionImpact : ExplosionImpact
    {
        public override bool CreatesExplosion => true;

        protected override AttackInfo GetDefaultImpact(Projectile projectile)
        {
            return new AttackInfo
            {
                Type = CreatureAttackType.C5_ATTACK_MAGIC
            };
        }

        public override async void GetExplosionAttack(Projectile projectile)
        {
            //centered only poison cloud
            var attack = projectile.KineticEnergy;
            var location = projectile.Location;
            //send event ==>

            while (true)
            {
                attack = Math.Max(1, Math.Min(attack >> 5, 4) + rand.Next(2));

                //TODO hit party
                //F324_aezz_CHAMPION_DamageAll_GetDamagedChampionCount(L0530_i_Attack, MASK0x0000_NO_WOUND, C0_ATTACK_NORMAL);

                F191_aayz_GROUP_GetDamageAllCreaturesOutcome(attack, location);

                if (attack >= 6)
                {
                    attack -= 3;
                    await Task.Delay(1000 / 6);
                }
                else
                {
                    break;
                }
            }
        }

        protected override int ModifyAttackyByResistance(int attack, ILiveEntity entity)
        {
            int L0390_i_PoisonResistance = 0;

            L0390_i_PoisonResistance = entity.GetProperty(PropertyFactory<AntiPoisonProperty>.Instance).Value;
            if (attack == 0 || L0390_i_PoisonResistance == 15)//_IMMUNE_TO_POISON if 15
            {
                return 0;
            }
            return ((attack + rand.Next(4)) << 3) / ++L0390_i_PoisonResistance;
        }
    }
}