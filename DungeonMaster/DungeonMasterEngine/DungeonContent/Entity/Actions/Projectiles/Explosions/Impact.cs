using System;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions.Projectiles.Explosions
{
    public abstract class Impact
    {
        protected static readonly Random rand = new Random();

        public abstract bool CreatesExplosion { get; }

        public AttackInfo GetImpact(Projectile projectile)
        {
            var info = GetDefaultImpact(projectile);
            info.Attack = ((info.Attack + projectile.KineticEnergy) >> 4) + 1;
            info.Attack += rand.Next((info.Attack >> 1) + 1) + rand.Next(4);
            info.Attack = MathHelper.Max(info.Attack >> 1, info.Attack - (32 - (projectile.Attack >> 3)));
            return info;
        }

        public virtual void GetExplosionAttack(Projectile projectile)
        {
        }

        protected abstract AttackInfo GetDefaultImpact(Projectile projectile);



        //int F216_xxxx_PROJECTILE_GetImpactAttack(Projectile P451_ps_Projectile, IGrabableItem P452_T_Thing)
        //{
        //    int A0483_ui_Attack;

        //    G366_i_ProjectilePoisonAttack = 0;
        //    G367_i_ProjectileAttackType = C3_ATTACK_BLUNT;
        //    int L0484_i_KineticEnergy = P451_ps_Projectile.KineticEnergy;
        //    if (P452_T_Thing != C15_THING_TYPE_EXPLOSION)
        //    {
        //        Weapon weapon = P452_T_Thing as Weapon;
        //        if (weapon != null)
        //        {
        //            var L0485_ps_WeaponInfo = weapon.FactoryType;
        //            A0483_ui_Attack = L0485_ps_WeaponInfo.KineticEnergy;
        //            G367_i_ProjectileAttackType = C3_ATTACK_BLUNT;
        //        }
        //        else
        //        {
        //            A0483_ui_Attack = rand.Next(4);
        //        }
        //        A0483_ui_Attack += P452_T_Thing.FactoryBase.Weight >> 1;
        //    }
        //    else
        //    {
        //        if (P452_T_Thing == C0xFF81_THING_EXPLOSION_SLIME)
        //        {
        //            A0483_ui_Attack = rand.Next(16);
        //            G366_i_ProjectilePoisonAttack = A0483_ui_Attack + 10;
        //            A0483_ui_Attack += rand.Next(32);
        //        }
        //        else
        //        {
        //            if (P452_T_Thing >= C0xFF83_THING_EXPLOSION_HARM_NON_MATERIAL)
        //            {
        //                G367_i_ProjectileAttackType = C5_ATTACK_MAGIC;
        //                if (P452_T_Thing == C0xFF86_THING_EXPLOSION_POISON_BOLT)
        //                {
        //                    G366_i_ProjectilePoisonAttack = L0484_i_KineticEnergy;
        //                    return 1;
        //                }
        //                return 0;
        //            }
        //            G367_i_ProjectileAttackType = C1_ATTACK_FIRE;
        //            A0483_ui_Attack = rand.Next(16) + rand.Next(16) + 10;
        //            if (P452_T_Thing == C0xFF82_THING_EXPLOSION_LIGHTNING_BOLT)
        //            {
        //                G367_i_ProjectileAttackType = C7_ATTACK_LIGHTNING;
        //                A0483_ui_Attack *= 5;
        //            }
        //        }
        //    }
        //    A0483_ui_Attack = ((A0483_ui_Attack + L0484_i_KineticEnergy) >> 4) + 1;
        //    A0483_ui_Attack += rand.Next((A0483_ui_Attack >> 1) + 1) + rand.Next(4);
        //    A0483_ui_Attack = MathHelper.Max(A0483_ui_Attack >> 1, A0483_ui_Attack - (32 - (P451_ps_Projectile.Attack >> 3)));
        //    return A0483_ui_Attack;
        //}
    }
}