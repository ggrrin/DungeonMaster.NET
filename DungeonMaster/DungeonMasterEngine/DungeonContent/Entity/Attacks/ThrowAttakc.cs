using System;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Attacks
{
    class ThrowAttakc : MeleeAttack
    {
        int F305_xxxx_CHAMPION_GetThrowingStaminaCost(IGrabableItem item)
        {
            int L0923_i_Weight;

            int L0924_i_StaminaCost = MathHelper.Clamp(L0923_i_Weight = item.Factory.Weight >> 1, 1, 10);//TODO sum weight of container
            while ((L0923_i_Weight -= 10) > 0)
            {
                L0924_i_StaminaCost += L0923_i_Weight >> 1;
            }
            return L0924_i_StaminaCost;
        }

        bool F328_nzzz_CHAMPION_IsObjectThrown(int P683_i_ChampionIndex, IStorageType P684_i_SlotIndex, int P685_i_Side)
        {
            IGrabableItem L0996_T_Thing;

            int L0993_i_KineticEnergy = F312_xzzz_CHAMPION_GetStrength(P684_i_SlotIndex);
            if ((L0996_T_Thing = attackProvider.Body.GetBodyStoratge(P684_i_SlotIndex).TakeItemFrom(0)) == null)
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
            //TODO
            //F212_mzzz_PROJECTILE_Create(L0996_T_Thing, G306_i_PartyMapX, G307_i_PartyMapY, M21_NORMALIZE(G308_i_PartyDirection + P685_i_Side), G308_i_PartyDirection, L0993_i_KineticEnergy, A0994_i_Attack, A0995_i_StepEnergy);
            //TODO
            //G311_i_ProjectileDisabledMovementTicks = 4;
            //G312_i_LastProjectileDisabledMovementDirection = G308_i_PartyDirection;
            //F292_arzz_CHAMPION_DrawState(P683_i_ChampionIndex);
            return true;
        }


        void F212_mzzz_PROJECTILE_Create(IGrabableItem P433_T_Thing, int P434_i_MapX, int P435_i_MapY, int P436_ui_Cell,
            int P437_i_Direction, int P438_uc_KineticEnergy, int P439_uc_Attack, int P440_uc_StepEnergy)
        {
            //THING L0466_T_ProjectileThing;
            //PROJECTILE* L0467_ps_Projectile;
            //EVENT L0468_s_Event;


            //L0466_T_ProjectileThing = M15_THING_WITH_NEW_CELL(L0466_T_ProjectileThing, P436_ui_Cell);
            //L0467_ps_Projectile = (PROJECTILE*)F156_afzz_DUNGEON_GetThingData(L0466_T_ProjectileThing);
            //L0467_ps_Projectile->Slot = P433_T_Thing;
            //L0467_ps_Projectile->KineticEnergy = F024_aatz_MAIN_GetMinimumValue(P438_uc_KineticEnergy, 255);
            //L0467_ps_Projectile->Attack = P439_uc_Attack;
            //if (G365_B_CreateLauncherProjectile)
            //{
            //    L0468_s_Event.A.A.Type = C49_EVENT_MOVE_PROJECTILE; /* Launcher projectiles can impact immediately */
            //}
            //else
            //{
            //    L0468_s_Event.A.A.Type = C48_EVENT_MOVE_PROJECTILE_IGNORE_IMPACTS; /* Projectiles created by champions or creatures ignore impacts on their first movement */
            //}
            //L0468_s_Event.A.A.Priority = 0;
            //L0468_s_Event.B.Slot = L0466_T_ProjectileThing;
            //L0468_s_Event.C.Projectile.MapX = P434_i_MapX;
            //L0468_s_Event.C.Projectile.MapY = P435_i_MapY;
            //L0468_s_Event.C.Projectile.StepEnergy = P440_uc_StepEnergy;
            //L0468_s_Event.C.Projectile.Direction = P437_i_Direction;
            //L0467_ps_Projectile->EventIndex = F238_pzzz_TIMELINE_AddEvent_GetEventIndex_COPYPROTECTIONE(&L0468_s_Event);
        }

        public void ApplyAttack(MapDirection direction)
        {
            throw new NotImplementedException();
        }

        public ThrowAttakc(HumanAttackFactoryBase factoryBase, ILiveEntity attackProvider) : base(factoryBase, attackProvider) { }
    }
}