using DungeonMasterEngine.DungeonContent.Actions;

namespace DungeonMasterEngine.DungeonContent.Projectiles.Impacts
{
    public class LightingBoltExplosionImpact : FireBaseExplosionImpact
    {
        public override bool CreatesExplosion => true; 

        protected override AttackInfo GetDefaultImpact(Projectile projectile)
        {
            int attack = rand.Next(16) + rand.Next(16) + 10;
            attack *= 5;
            return new AttackInfo
            {
                Attack = attack,
                Type = CreatureAttackType.C7_ATTACK_LIGHTNING
            };
        }

        public override void FinishImpact(Projectile projectile)
        {
            var attack = projectile.KineticEnergy >> 1;
            var delay = 1 * 1000 / 6; //rebirth1 5
            var location = projectile.Location;
            var attackBefore = attack;
            //send event ==>

            attack = (attack >> 1) + 1;
            attack += rand.Next(attack) + 1;

            if ((attack >>= 1) > 0)
            {
                //TODO hit champions
                //if ((G272_i_CurrentMapIndex == G309_i_PartyMapIndex) && (AP443_ui_ProjectileMapX == G306_i_PartyMapX) && (AP444_ui_ProjectileMapY == G307_i_PartyMapY))
                //{
                //    F324_aezz_CHAMPION_DamageAll_GetDamagedChampionCount(P442_ui_Attack, MASK0x0001_READY_HAND | MASK0x0002_ACTION_HAND | MASK0x0004_HEAD | MASK0x0008_TORSO | MASK0x0010_LEGS | MASK0x0020_FEET, C1_ATTACK_FIRE);
                //    return;
                //}


                F191_aayz_GROUP_GetDamageAllCreaturesOutcome(attack, location);
            }

            attackBefore = (attackBefore >> 1) + 1;
            attackBefore += rand.Next(attackBefore) + 1;
            if ((attackBefore >>= 1) > 0)
            {
                //TODO try destroy door (attackBefore)
            }
        }
    }
}