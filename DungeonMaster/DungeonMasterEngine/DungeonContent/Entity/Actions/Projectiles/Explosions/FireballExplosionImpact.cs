using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public abstract class FireBaseExplosionImpact : ExplosionImpact
    {

        protected override int ModifyAttackyByResistance(int attack, ILiveEntity entity)
        {
            var antiFire = entity.GetProperty(PropertyFactory<AntiFireProperty>.Instance);
            if (antiFire.Value == 15)
                return 0;

            if (entity.GetProperty(PropertyFactory<NonMaterialProperty>.Instance).Value == 1)
                attack >>= 2;

            attack -= rand.Next((antiFire.Value << 1) + 1);
            return attack;
        }

        public abstract override void GetExplosionAttack(Projectile projectile);
    }

    public class FireballExplosionImpact : FireBaseExplosionImpact
    {
        public override bool CreatesExplosion => true;

        protected override AttackInfo GetDefaultImpact(Projectile projectile)
        {
            return new AttackInfo
            {
                Attack = rand.Next(16) + rand.Next(16) + 10,
                Type = CreatureAttackType.C1_ATTACK_FIRE
            };
        }

        public override void GetExplosionAttack(Projectile projectile)
        {
            var attack = projectile.KineticEnergy;
            var delay = 1 * 1000 / 6; //rebirth1 5
            var location = projectile.Location;
            var attackBefore = attack;
            //send event ==>

            attack = (attack >> 1) + 1;
            attack += rand.Next(attack) + 1;

            //TODO hit champions
            //if ((G272_i_CurrentMapIndex == G309_i_PartyMapIndex) && (AP443_ui_ProjectileMapX == G306_i_PartyMapX) && (AP444_ui_ProjectileMapY == G307_i_PartyMapY))
            //{
            //    F324_aezz_CHAMPION_DamageAll_GetDamagedChampionCount(P442_ui_Attack, MASK0x0001_READY_HAND | MASK0x0002_ACTION_HAND | MASK0x0004_HEAD | MASK0x0008_TORSO | MASK0x0010_LEGS | MASK0x0020_FEET, C1_ATTACK_FIRE);
            //    return;
            //}


            F191_aayz_GROUP_GetDamageAllCreaturesOutcome(attack, location);


            attackBefore = (attackBefore >> 1) + 1;
            attackBefore += rand.Next(attackBefore) + 1;
            //TODO try destroy door (attackBefore)
        }


    }
}