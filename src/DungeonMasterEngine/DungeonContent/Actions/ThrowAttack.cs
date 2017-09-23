using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Projectiles;
using DungeonMasterEngine.DungeonContent.Projectiles.Impacts;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actions
{
    class ThrowAttack : MeleeAttack<ThrowActionFactory>
    {
        public IStorageType P684_i_SlotIndex { get; }

        public ThrowAttack(ThrowActionFactory factoryBase, ILiveEntity attackProvider, IStorageType p684ISlotIndex) : base(factoryBase, attackProvider)
        {
            P684_i_SlotIndex = p684ISlotIndex;
        }

        int F305_xxxx_CHAMPION_GetThrowingStaminaCost(IGrabableItem item)
        {
            int L0923_i_Weight;

            int L0924_i_StaminaCost = MathHelper.Clamp(L0923_i_Weight = item.FactoryBase.Weight >> 1, 1, 10);//TODO sum weight of container
            while ((L0923_i_Weight -= 10) > 0)
            {
                L0924_i_StaminaCost += L0923_i_Weight >> 1;
            }
            return L0924_i_StaminaCost;
        }

        public override int Apply(MapDirection direction)
        {
            F328_nzzz_CHAMPION_IsObjectThrown(direction);
            return Factory.Fatigue;
        }

        bool F328_nzzz_CHAMPION_IsObjectThrown(MapDirection P685_i_Side)
        {
            IGrabableItem L0996_T_Thing;

            int L0993_i_KineticEnergy = F312_xzzz_CHAMPION_GetStrength(attackProvider, P684_i_SlotIndex);
            if ((L0996_T_Thing = attackProvider.Body.GetBodyStorage(P684_i_SlotIndex).TakeItemFrom(0)) == null)
            {
                return false;
            }
            attackProvider.GetProperty(PropertyFactory<StaminaProperty>.Instance).Value -= F305_xxxx_CHAMPION_GetThrowingStaminaCost(L0996_T_Thing);
            //F330_szzz_CHAMPION_DisableAction(P683_i_ChampionIndex, 4); //TODO disable actiono 
            int A0994_i_Experience = 8;
            int A0995_i_WeaponKineticEnergy = 1;
            if (L0996_T_Thing is Weapon)
            {
                A0994_i_Experience += 4;
                var L0998_ps_WeaponInfo = (Weapon)L0996_T_Thing;

                if (L0998_ps_WeaponInfo.FactoryType.Class <= WeaponClass.C012_CLASS_POISON_DART)
                {
                    A0994_i_Experience += (A0995_i_WeaponKineticEnergy = L0998_ps_WeaponInfo.FactoryType.KineticEnergy) >> 2;
                }
            }
            var skill = attackProvider.GetSkill(SkillFactory<ThrowSkill>.Instance);
            skill.AddExperience(A0994_i_Experience);
            L0993_i_KineticEnergy += A0995_i_WeaponKineticEnergy;
            int A0995_i_SkillLevel = skill.SkillLevel;
            L0993_i_KineticEnergy += rand.Next(16) + (L0993_i_KineticEnergy >> 1) + A0995_i_SkillLevel;
            int A0994_i_Attack = MathHelper.Clamp((A0995_i_SkillLevel << 3) + rand.Next(32), 40, 200);
            int A0995_i_StepEnergy = MathHelper.Max(5, 11 - A0995_i_SkillLevel);

            F212_mzzz_PROJECTILE_Create(L0996_T_Thing, P685_i_Side, L0993_i_KineticEnergy, A0994_i_Attack, A0995_i_StepEnergy);

            //TODO //G311_i_ProjectileDisabledMovementTicks = 4;
            return true;
        }

        void F212_mzzz_PROJECTILE_Create(IGrabableItem P433_T_Thing, MapDirection P437_i_Direction, int P438_uc_KineticEnergy, int P439_uc_Attack, int P440_uc_StepEnergy)
        {
            Projectile L0467_ps_Projectile;
            Weapon weapon = P433_T_Thing as Weapon;
            if (weapon == null)
            {
                L0467_ps_Projectile = new ImpactProjectile(P438_uc_KineticEnergy, P440_uc_StepEnergy, P439_uc_Attack, new ThrowImpact<IGrabableItem>(P433_T_Thing));
            }
            else
            {
                L0467_ps_Projectile = new ImpactProjectile(P438_uc_KineticEnergy, P440_uc_StepEnergy, P439_uc_Attack, new WeaponImpact(weapon));
            }
            L0467_ps_Projectile.Renderer = Factory.Renderers.GetProjectileSpellRenderer(L0467_ps_Projectile, P433_T_Thing.FactoryBase.Renderer.Texture);
            L0467_ps_Projectile.Run(attackProvider.Location, P437_i_Direction);
        }

    }
}