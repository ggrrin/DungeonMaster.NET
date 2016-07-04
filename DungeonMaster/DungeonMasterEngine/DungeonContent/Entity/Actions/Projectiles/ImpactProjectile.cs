using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public class ImpactProjectile : Projectile
    {
        public Impact Impact { get; }

        public ImpactProjectile(int kineticEnergy, int stepEnergy, int attack, Impact impact) : base(kineticEnergy, stepEnergy, attack)
        {
            Impact = impact;
        }

        protected override bool TryApplyBeforeMoving()
        {
            return F217_xxxx_PROJECTILE_HasImpactOccured();
        }

        bool F217_xxxx_PROJECTILE_HasImpactOccured()
        {
            //potionException(L0486_T_ProjectileAssociatedThing, ref L0491_ps_Group, ref L0492_ps_Potion, ref L0498_T_ExplosionThing, ref L0508_i_PotionPower, ref L0509_B_RemovePotion, L0510_i_ProjectileAssociatedThingType);

            bool L0505_B_CreateExplosionOnImpact = Impact.CreatesExplosion;

            //TODO break doors
            //L0488_i_Attack = F216_xxxx_PROJECTILE_GetImpactAttack(L0490_ps_Projectile, L0486_T_ProjectileAssociatedThing) + 1;
            //F232_dzzz_GROUP_IsDoorDestroyedByAttack(AP454_i_ProjectileTargetMapX, AP455_i_ProjectileTargetMapY, L0488_i_Attack + M02_RANDOM(L0488_i_Attack), FALSE, 0);

            var matchingEnemies = Location.Tile.LayoutManager.GetEntities(Location.Space).ToArray();
            if (matchingEnemies.Length == 0)
                return false;

            var closestSpace = GetClosestSpace(Location, matchingEnemies.Select(x => x.Location));
            var enemy = matchingEnemies.First(x => x.Location == closestSpace);

            AttackInfo impact = Impact.GetImpact(this);
            int L0488_i_Attack = (impact.Attack << 6) / enemy.GetProperty(PropertyFactory<DefenseProperty>.Instance).Value;
            if (L0488_i_Attack > 0)
            {
                int adjustedPoisonEffect = F192_ayzz_GROUP_GetResistanceAdjustedPoisonAttack(enemy, impact.PoisonAttack);
                var health = enemy.GetProperty(PropertyFactory<HealthProperty>.Instance);
                health.Value -= L0488_i_Attack + adjustedPoisonEffect;
                // TODO itemAbsorbtion();
            }

            //TODO champions
            //L0488_i_Attack = F216_xxxx_PROJECTILE_GetImpactAttack(L0490_ps_Projectile, L0486_T_ProjectileAssociatedThing);
            //if (L0489_i_ChampionAttack && F321_AA29_CHAMPION_AddPendingDamageAndWounds_GetDamage(AP456_i_ChampionIndex, L0488_i_Attack, MASK0x0004_HEAD | MASK0x0008_TORSO, G367_i_ProjectileAttackType) && G366_i_ProjectilePoisonAttack && M05_RANDOM(2))
            //{
            //    F322_lzzz_CHAMPION_Poison(AP456_i_ChampionIndex, G366_i_ProjectilePoisonAttack);
            //}

            //create explosion
            if (L0505_B_CreateExplosionOnImpact)
            {
                //L0486_T_ProjectileAssociatedThing = L0498_T_ExplosionThing;
                //A0507_ui_ExplosionAttack = L0508_i_PotionPower;
                Impact.GetExplosionAttack(this);
            }
            return true;
        }
        int F192_ayzz_GROUP_GetResistanceAdjustedPoisonAttack(ILiveEntity creature, int P381_i_PoisonAttack)
        {
            int L0390_i_PoisonResistance = 0;

            L0390_i_PoisonResistance = creature.GetProperty(PropertyFactory<AntiPoisonProperty>.Instance).Value;
            if (P381_i_PoisonAttack == 0 || L0390_i_PoisonResistance == 15)//_IMMUNE_TO_POISON if 15
            {
                return 0;
            }
            return ((P381_i_PoisonAttack + rand.Next(4)) << 3) / ++L0390_i_PoisonResistance;
        }
        
    }
}