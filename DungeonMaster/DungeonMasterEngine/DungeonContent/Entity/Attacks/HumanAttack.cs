using System;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Attacks
{

    class ThrowAttakc : HumanAttack
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

        public ThrowAttakc(HumanAttackFactory factory, ILiveEntity attackProvider) : base(factory, attackProvider) { }
    }

    class HumanAttack : IAttack
    {
        protected static Random rand = new Random();

        private readonly HumanAttackFactory factory;
        protected readonly ILiveEntity attackProvider;
        private Weapon weapon;

        public HumanAttack(HumanAttackFactory factory, ILiveEntity attackProvider)
        {
            this.factory = factory;
            this.attackProvider = attackProvider;
        }

        private ILiveEntity GetAccesibleEnemies(MapDirection partyDirection)
        {
            //TODO rework using rectangle intersection
            var targetTile = attackProvider.Location.Tile.Neighbours.GetTile(partyDirection);
            var enemy = targetTile?.LayoutManager.Entities.Where(e => attackProvider.RelationManager.IsEnemy(e.RelationManager.RelationToken)) //todo or otherwise
                .MinObj(c => Vector3.Distance(c.Position, attackProvider.Position));

            if (enemy != null)
            {
                bool isInFirstRow = attackProvider.Location.Space.Sides.Contains(partyDirection);
                if (!isInFirstRow)
                {
                    bool someBodyInFirstRow = null != attackProvider.Location.Tile.LayoutManager.Entities.FirstOrDefault(e => e.Location.Space.Sides.Contains(partyDirection));
                    if (someBodyInFirstRow)
                        return null;
                }
            }
            return enemy;
        }


        bool F402_xxxx_MENUS_IsMeleeActionPerformed(MapDirection direction)
        {
            var enemy = GetAccesibleEnemies(direction); //aka ismeleeActionPerformed
            if (enemy != null)
            {

                if (false) //TODO (P775_i_ActionIndex == C024_ACTION_DISRUPT) && !M07_GET(F144_amzz_DUNGEON_GetCreatureAttributes(G517_T_ActionTargetGroupThing), MASK0x0040_NON_MATERIAL))
                {
                }
                else
                {
                    ActionProbabilityInfo A1237_i_ActionHitProbability = new ActionProbabilityInfo(factory.HitProbability); //G493_auc_Graphic560_ActionHitProbability[P775_i_ActionIndex];
                    int A1236_ui_ActionDamageFactor = factory.Damage;  //G492_auc_Graphic560_ActionDamageFactor[P775_i_ActionIndex];
                    A1237_i_ActionHitProbability.HitNonmaterial = attackProvider.GetProperty(PropertyFactory<NonMaterialProperty>.Instance).MaxValue > 0;
                    //if ((F033_aaaz_OBJECT_GetIconIndex(P774_ps_Champion->Slots[C01_SLOT_ACTION_HAND]) == C040_ICON_WEAPON_VORPAL_BLADE) || (P775_i_ActionIndex == C024_ACTION_DISRUPT))
                    //{
                    //    M08_SET(A1237_i_ActionHitProbability, MASK0x8000_HIT_NON_MATERIAL_CREATURES);
                    //}
                    F231_izzz_GROUP_GetMeleeActionDamage(enemy, A1237_i_ActionHitProbability, A1236_ui_ActionDamageFactor/*, P778_i_SkillIndex*/);
                    return true;
                }
            }
            T402_010:
            return false;
        }

        public async void ApplyAttack(MapDirection direction)
        {
            var fatigue = factory.Fatigue;
            var requiredSkill = attackProvider.GetSkill(factory.SkillIndex);
            var requiredStamina = factory.Stamina + rand.Next(2);

            if (F402_xxxx_MENUS_IsMeleeActionPerformed(direction))
            {
                fatigue >>= 1;
            }

            //AfterSwitch
            if (fatigue > 0)
            {
                await Task.Delay(fatigue);
            }
            if (requiredStamina > 0)
            {
                attackProvider.GetProperty(PropertyFactory<StaminaProperty>.Instance).Value -= requiredStamina;
            }
            if (factory.ExperienceGain > 0)
            {
                requiredSkill.AddExperience(factory.ExperienceGain);
            }
        }

        int F231_izzz_GROUP_GetMeleeActionDamage(ILiveEntity enemy, ActionProbabilityInfo P501_i_ActionHitProbability, int P502_ui_ActionDamageFactor)
        {
            int L0565_i_Damage;
            int L0568_i_Defense = 0;
            bool L0570_B_ActionHitsNonMaterialCreatures;

            int L0567_i_DoubledMapDifficulty = attackProvider.Location.Tile.LevelIndex;//Todo map difficulty
                                                                                       //commented L0571_i_ActionHandObjectIconIndex = F033_aaaz_OBJECT_GetIconIndex(P495_ps_Champion->Slots[C01_SLOT_ACTION_HAND]);
                                                                                       //if (L0570_B_ActionHitsNonMaterialCreatures = M07_GET(P501_i_ActionHitProbability, MASK0x8000_HIT_NON_MATERIAL_CREATURES))
            if (L0570_B_ActionHitsNonMaterialCreatures = P501_i_ActionHitProbability.HitNonmaterial)
            {
                //M09_CLEAR(P501_i_ActionHitProbability, MASK0x8000_HIT_NON_MATERIAL_CREATURES);
                P501_i_ActionHitProbability.HitNonmaterial = false;
            }
            //if ((!M07_GET(L0572_ps_CreatureInfo->Attributes, MASK0x0040_NON_MATERIAL) || L0570_B_ActionHitsNonMaterialCreatures) &&
            //    ((F311_wzzz_CHAMPION_GetDexterity(P495_ps_Champion) > (M03_RANDOM(32) + L0572_ps_CreatureInfo->Dexterity + L0567_i_DoubledMapDifficulty - 16)) ||
            //     (!M04_RANDOM(4)) ||
            //     (F308_vzzz_CHAMPION_IsLucky(P495_ps_Champion, 75 - P501_i_ActionHitProbability))))

            if ((enemy.GetProperty(PropertyFactory<NonMaterialProperty>.Instance).Value == 0 || L0570_B_ActionHitsNonMaterialCreatures) &&//  !M07_GET(L0572_ps_CreatureInfo->Attributes, MASK0x0040_NON_MATERIAL) || L0570_B_ActionHitsNonMaterialCreatures) &&
                ((attackProvider.GetProperty(PropertyFactory<DextrityProperty>.Instance/*P495_ps_Champion*/).MaxValue > (rand.Next(32) + enemy.GetProperty(PropertyFactory<DextrityProperty>.Instance).MaxValue + L0567_i_DoubledMapDifficulty - 16)) ||
                 (rand.Next(4) == 0) || (F308_vzzz_CHAMPION_IsLucky(/*P495_ps_Champion,*/ 75 - P501_i_ActionHitProbability.Value))))
            {
                if ((L0565_i_Damage = F312_xzzz_CHAMPION_GetStrength(/*P496_i_ChampionIndex, C01_SLOT_ACTION_HAND,*/ ActionHandStorageType.Instance)) > 0)
                {
                    //goto T231_009;
                }
                else//
                {//
                    L0565_i_Damage += rand.Next((L0565_i_Damage >> 1) + 1);
                    L0565_i_Damage = (/*(long)*/L0565_i_Damage * /*(long)*/P502_ui_ActionDamageFactor) >> 5;
                    L0568_i_Defense = rand.Next(32) + /*L0572_ps_CreatureInfo->Defense*/ enemy.GetProperty(PropertyFactory<DefenseProperty>.Instance).MaxValue + L0567_i_DoubledMapDifficulty;
                    //TODO item property modification
                    //if (L0571_i_ActionHandObjectIconIndex == C039_ICON_WEAPON_DIAMOND_EDGE)
                    //{
                    //    L0568_i_Defense -= L0568_i_Defense >> 2;
                    //}
                    //else
                    //{
                    //    if (L0571_i_ActionHandObjectIconIndex == C043_ICON_WEAPON_HARDCLEAVE_EXECUTIONER)
                    //    {
                    //        L0568_i_Defense -= L0568_i_Defense >> 3;
                    //    }
                    //}
                }//

                int L0566_i_Damage;
                if ((L0566_i_Damage = L0565_i_Damage = rand.Next(32) + L0565_i_Damage - L0568_i_Defense) <= 1)
                {
                    T231_009:
                    if ((L0565_i_Damage = rand.Next(4)) > 0)
                    {
                        goto T231_015;
                    }
                    L0565_i_Damage++;
                    if (((L0566_i_Damage += rand.Next(16)) > 0) || (rand.Next(2)) > 0)
                    {
                        L0565_i_Damage += rand.Next(4);
                        if (rand.Next(4) == 0)
                        {
                            L0565_i_Damage += MathHelper.Max(0, L0566_i_Damage + rand.Next(16)); //F025_aatz_MAIN_GetMaximumValue(0, L0566_i_Damage + M03_RANDOM(16));
                        }
                    }
                }
                L0565_i_Damage >>= 1;
                L0565_i_Damage += rand.Next(L0565_i_Damage) + rand.Next(4);
                L0565_i_Damage += rand.Next(L0565_i_Damage);
                L0565_i_Damage >>= 2;
                L0565_i_Damage += rand.Next(4) + 1;
                //TODO weapon property modification
                //if ((L0571_i_ActionHandObjectIconIndex == C040_ICON_WEAPON_VORPAL_BLADE) && !M07_GET(L0572_ps_CreatureInfo->Attributes, MASK0x0040_NON_MATERIAL) && !(L0565_i_Damage >>= 1))
                //{
                //    goto T231_015;
                //}
                if (rand.Next(64) < attackProvider.GetSkill(factory.SkillIndex).SkillLevel)//  F303_AA09_CHAMPION_GetSkillLevel(/*P496_i_ChampionIndex,*/ P503_i_SkillIndex))
                {
                    L0565_i_Damage += L0565_i_Damage + 10;
                }
                //L0569_i_Outcome = F190_zzzz_GROUP_GetDamageCreatureOutcome(P497_ps_Group, P498_i_CreatureIndex, P499_i_MapX, P500_i_MapY, L0565_i_Damage, true);
                enemy.GetProperty(PropertyFactory<HealthProperty>.Instance).Value -= L0566_i_Damage;
                //F304_apzz_CHAMPION_AddSkillExperience(/*P496_i_ChampionIndex,*/ P503_i_SkillIndex, (L0565_i_Damage * L0572_ps_CreatureInfo.Experience /*M58_EXPERIENCE(L0572_ps_CreatureInfo->Properties)*/ >> 4) + 3);
                var experienceGain = enemy.GetProperty(PropertyFactory<ExperienceProperty>.Instance).Value;
                attackProvider.GetSkill(factory.SkillIndex).AddExperience((L0565_i_Damage * experienceGain >> 4) + 3);

                attackProvider.GetProperty(PropertyFactory<StaminaProperty>.Instance).Value -= /*P496_i_ChampionIndex, */ rand.Next(4) + 4;
                goto T231_016;
            }
            T231_015:
            L0565_i_Damage = 0;
            //comment L0569_i_Outcome = C0_OUTCOME_KILLED_NO_CREATURES_IN_GROUP;
            attackProvider.GetProperty(PropertyFactory<StaminaProperty>.Instance).Value -= /*P496_i_ChampionIndex, */ rand.Next(2) + 2;
            T231_016:
            //commented
            //F292_arzz_CHAMPION_DrawState(P496_i_ChampionIndex);
            //if (L0569_i_Outcome != C2_OUTCOME_KILLED_ALL_CREATURES_IN_GROUP)
            //{
            //    F209_xzzz_GROUP_ProcessEvents29to41(P499_i_MapX, P500_i_MapY, CM1_EVENT_CREATE_REACTION_EVENT_31_PARTY_IS_ADJACENT, 0);
            //}
            return L0565_i_Damage;
        }

        bool F308_vzzz_CHAMPION_IsLucky(int P646_ui_Percentage)
        {
            if (rand.Next(2) != 0 && (rand.Next(100) > P646_ui_Percentage))
                return true;

            var luckProperty = attackProvider.GetProperty(PropertyFactory<LuckProperty>.Instance);
            bool AP646_ui_IsLucky = rand.Next(luckProperty.MaxValue) > P646_ui_Percentage;
            luckProperty.BaseValue += AP646_ui_IsLucky ? -2 : 2;
            return AP646_ui_IsLucky;
        }

        protected int F312_xzzz_CHAMPION_GetStrength(IStorageType P651_i_SlotIndex)
        {
            int L0935_i_Strength = rand.Next(16) + attackProvider.GetProperty(PropertyFactory<StrengthProperty>.Instance).MaxValue;
            var L0938_T_Thing = attackProvider.Body.GetStorage(P651_i_SlotIndex).Storage.First();
            //if ((A0936_ui_ObjectWeight = F140_yzzz_DUNGEON_GetObjectWeight(L0938_T_Thing)) <= (A0937_ui_OneSixteenthMaximumLoad = F309_awzz_CHAMPION_GetMaximumLoad(L0939_ps_Champion) >> 4))
            int A0936_ui_ObjectWeight = (int)L0938_T_Thing.Factory.Weight;
            int A0937_ui_OneSixteenthMaximumLoad = attackProvider.GetProperty(PropertyFactory<LoadProperty>.Instance).MaxValue >> 4;
            if (A0936_ui_ObjectWeight <= A0937_ui_OneSixteenthMaximumLoad)
            {
                L0935_i_Strength += A0936_ui_ObjectWeight - 12;
            }
            else
            {
                int L0941_i_LoadThreshold;
                if (A0936_ui_ObjectWeight <= (L0941_i_LoadThreshold = A0937_ui_OneSixteenthMaximumLoad + ((A0937_ui_OneSixteenthMaximumLoad - 12) >> 1)))
                {
                    L0935_i_Strength += (A0936_ui_ObjectWeight - A0937_ui_OneSixteenthMaximumLoad) >> 1;
                }
                else
                {
                    L0935_i_Strength -= (A0936_ui_ObjectWeight - L0941_i_LoadThreshold) << 1;
                }
            }
            //if (M12_TYPE(L0938_T_Thing) == C05_THING_TYPE_WEAPON)
            if (L0938_T_Thing is Weapon)
            {
                //#ifdef C01_COMPILE_DM10aEN_DM10bEN /* CHANGE2_01_OPTIMIZATION Inline code replaced by function calls */
                //                L0940_ps_WeaponInfo = &G238_as_Graphic559_WeaponInfo[((WEAPON*)G284_apuc_ThingData[C05_THING_TYPE_WEAPON])[M13_INDEX(L0938_T_Thing)].Type];
                //#endif
                //#ifdef C15_COMPILE_DM11EN_DM12EN_DM12GE_DM13aFR_DM13bFR_CSB20EN_CSB21EN /* CHANGE2_01_OPTIMIZATION Inline code replaced by function calls */
                var L0940_ps_WeaponInfo = L0938_T_Thing as Weapon;
                //#endif
                L0935_i_Strength += L0940_ps_WeaponInfo.FactoryType.Strength;
                int A0936_ui_SkillLevel = 0;
                var A0937_ui_Class = L0940_ps_WeaponInfo.FactoryType.Class;
                if ((A0937_ui_Class == WeaponClass.C000_CLASS_SWING_WEAPON) || (A0937_ui_Class == WeaponClass.C002_CLASS_DAGGER_AND_AXES))
                {
                    A0936_ui_SkillLevel = attackProvider.GetSkill(SkillFactory<SwingSkill>.Instance).SkillLevel;
                }
                if ((A0937_ui_Class != WeaponClass.C000_CLASS_SWING_WEAPON) && (A0937_ui_Class < WeaponClass.C016_CLASS_FIRST_BOW))
                {
                    A0936_ui_SkillLevel += attackProvider.GetSkill(SkillFactory<ThrowSkill>.Instance).SkillLevel;
                }
                if ((A0937_ui_Class >= WeaponClass.C016_CLASS_FIRST_BOW) && (A0937_ui_Class < WeaponClass.C112_CLASS_FIRST_MAGIC_WEAPON))
                {
                    A0936_ui_SkillLevel += attackProvider.GetSkill(SkillFactory<ShootSkill>.Instance).SkillLevel;
                }
                L0935_i_Strength += A0936_ui_SkillLevel << 1;
            }
            L0935_i_Strength = F306_xxxx_CHAMPION_GetStaminaAdjustedValue(attackProvider.GetProperty(PropertyFactory<StaminaProperty>.Instance),/*L0939_ps_Champion,*/ L0935_i_Strength);
            //if (M07_GET(L0939_ps_Champion->Wounds, (P651_i_SlotIndex == C00_SLOT_READY_HAND) ? MASK0x0001_READY_HAND : MASK0x0002_ACTION_HAND))
            if (attackProvider.Body.GetBodyStoratge(P651_i_SlotIndex).IsWound)
            {
                L0935_i_Strength >>= 1;
            }
            //return F026_a003_MAIN_GetBoundedValue(0, L0935_i_Strength >> 1, 100);
            return MathHelper.Clamp(L0935_i_Strength >> 1, 0, 100);
        }

        /// <summary>
        /// Adjust damage if stamina is at low level
        /// </summary>
        /// <param name="P641_i_Value"></param>
        /// <returns></returns>
        public static int F306_xxxx_CHAMPION_GetStaminaAdjustedValue(IProperty staminaProperty, int P641_i_Value)
        {
            int L0925_i_CurrentStamina;
            int L0926_i_HalfMaximumStamina;

            //if ((L0925_i_CurrentStamina = P640_ps_Champion->CurrentStamina) < (L0926_i_HalfMaximumStamina = P640_ps_Champion->MaximumStamina >> 1))
            L0925_i_CurrentStamina = staminaProperty.Value;
            L0926_i_HalfMaximumStamina = staminaProperty.MaxValue >> 1;
            if (L0925_i_CurrentStamina < L0926_i_HalfMaximumStamina)
            {
                return (P641_i_Value >>= 1) + (int)(((long)P641_i_Value * (long)L0925_i_CurrentStamina) / L0926_i_HalfMaximumStamina);
            }
            return P641_i_Value;
        }
    }
}