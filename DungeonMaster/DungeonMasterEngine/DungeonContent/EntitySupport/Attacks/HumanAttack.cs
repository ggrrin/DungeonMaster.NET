using System;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;
using DungeonMasterEngine.DungeonContent.EntitySupport.Skills;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Attacks
{
    class HumanAttack : IAttack
    {
        private static Random rand = new Random();

        private readonly HumanAttackFactory factory;
        private readonly IEntity attackProvider;
        private Weapon weapon;

        public HumanAttack(HumanAttackFactory factory, IEntity attackProvider)
        {
            this.factory = factory;
            this.attackProvider = attackProvider;
        }

        private IEntity GetAccesibleEnemies(MapDirection partyDirection)
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


        bool F402_xxxx_MENUS_IsMeleeActionPerformed(/*int P773_i_ChampionIndex, Champion P774_ps_Champion,*/MapDirection direction,/* int P775_i_ActionIndex, int P776_i_TargetMapX,int P777_i_TargetMapY,*/SkillInfo P778_i_SkillIndex)
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
                    A1237_i_ActionHitProbability.HitNonmaterial = attackProvider.GetProperty(NonMaterialPropertyFactory.Instance).CurrentValue > 0;
                    //if ((F033_aaaz_OBJECT_GetIconIndex(P774_ps_Champion->Slots[C01_SLOT_ACTION_HAND]) == C040_ICON_WEAPON_VORPAL_BLADE) || (P775_i_ActionIndex == C024_ACTION_DISRUPT))
                    //{
                    //    M08_SET(A1237_i_ActionHitProbability, MASK0x8000_HIT_NON_MATERIAL_CREATURES);
                    //}
                    var G513_i_ActionDamage = F231_izzz_GROUP_GetMeleeActionDamage(enemy, A1237_i_ActionHitProbability, A1236_ui_ActionDamageFactor, P778_i_SkillIndex);
                    return true;
                }
            }
            T402_010:
            return false;
        }

        public async void ApplyAttack(MapDirection direction)
        {
            var L1249_ui_ActionDisabledTicks = factory.Fatigue; // G491_auc_Graphic560_ActionDisabledTicks[P788_i_ActionIndex];
            var L1254_i_ActionSkillIndex = new SkillInfo(factory.SkillIndex);// G496_auc_Graphic560_ActionSkillIndex[P788_i_ActionIndex];
            var L1253_i_ActionStamina = factory.Stamina + rand.Next(2);// G494_auc_Graphic560_ActionStamina[P788_i_ActionIndex] + M05_RANDOM(2);
            var L1255_i_ActionExperienceGain = factory.ExperienceGain;// G497_auc_Graphic560_ActionExperienceGain[P788_i_ActionIndex];

            if (F402_xxxx_MENUS_IsMeleeActionPerformed(direction, new SkillInfo(factory.SkillIndex)))
            {
                L1255_i_ActionExperienceGain >>= 1;
                L1249_ui_ActionDisabledTicks >>= 1;
            }


            //AfterSwitch
            if (L1249_ui_ActionDisabledTicks > 0)
            {
                await Task.Delay(L1249_ui_ActionDisabledTicks); //F330_szzz_CHAMPION_DisableAction(P787_i_ChampionIndex, L1249_ui_ActionDisabledTicks);
            }
            if (L1253_i_ActionStamina > 0)
            {
                F325_bzzz_CHAMPION_DecrementStamina(/*P787_i_ChampionIndex,*/ L1253_i_ActionStamina);
            }
            if (L1255_i_ActionExperienceGain > 0)
            {
                F304_apzz_CHAMPION_AddSkillExperience(/*P787_i_ChampionIndex,*/ L1254_i_ActionSkillIndex, L1255_i_ActionExperienceGain);
            }
        }

        int F231_izzz_GROUP_GetMeleeActionDamage(/*IEntity P495_ps_Champion, int P496_i_ChampionIndex,*/ IEntity P497_ps_Group, /*int P498_i_CreatureIndex, int P499_i_MapX, int P500_i_MapY,
           */ ActionProbabilityInfo P501_i_ActionHitProbability, int P502_ui_ActionDamageFactor, SkillInfo P503_i_SkillIndex)
        {
            int L0565_i_Damage;
            int L0566_i_Damage;
            int L0567_i_DoubledMapDifficulty;
            int L0568_i_Defense = 0;
            int L0569_i_Outcome;
            bool L0570_B_ActionHitsNonMaterialCreatures;
            int L0571_i_ActionHandObjectIconIndex;
            CreatureInfo L0572_ps_CreatureInfo;

            //if (P496_i_ChampionIndex >= G305_ui_PartyChampionCount) 
            if (attackProvider == null)
            {
                return 0;
            }
            //TODO
            //if (!P495_ps_Champion->CurrentHealth)
            //{  
            //        return 0;
            //}
            L0567_i_DoubledMapDifficulty = attackProvider.Location.Tile.LevelIndex;//TODO user defficulty G269_ps_CurrentMap->C.Difficulty << 1;
            L0572_ps_CreatureInfo = P497_ps_Group.Type;  //&G243_as_Graphic559_CreatureInfo[P497_ps_Group->Type];
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

            if ((!L0572_ps_CreatureInfo.IsNonMaterial || L0570_B_ActionHitsNonMaterialCreatures) &&//  !M07_GET(L0572_ps_CreatureInfo->Attributes, MASK0x0040_NON_MATERIAL) || L0570_B_ActionHitsNonMaterialCreatures) &&
                ((F311_wzzz_CHAMPION_GetDexterity(/*P495_ps_Champion*/) > (rand.Next(32) + P497_ps_Group.GetProperty(DextrityPropertyFactory.Instance).CurrentValue + L0567_i_DoubledMapDifficulty - 16)) ||
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
                    L0568_i_Defense = rand.Next(32) + /*L0572_ps_CreatureInfo->Defense*/ P497_ps_Group.GetProperty(DefensePropertyFactory.Instance).CurrentValue + L0567_i_DoubledMapDifficulty;
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
                if (rand.Next(64) < F303_AA09_CHAMPION_GetSkillLevel(/*P496_i_ChampionIndex,*/ P503_i_SkillIndex))
                {
                    L0565_i_Damage += L0565_i_Damage + 10;
                }
                //L0569_i_Outcome = F190_zzzz_GROUP_GetDamageCreatureOutcome(P497_ps_Group, P498_i_CreatureIndex, P499_i_MapX, P500_i_MapY, L0565_i_Damage, true);
                P497_ps_Group.GetProperty(HealthPropertyFactory.Instance).StoredValue -= L0566_i_Damage;
                F304_apzz_CHAMPION_AddSkillExperience(/*P496_i_ChampionIndex,*/ P503_i_SkillIndex, (L0565_i_Damage * L0572_ps_CreatureInfo.Experience /*M58_EXPERIENCE(L0572_ps_CreatureInfo->Properties)*/ >> 4) + 3);
                F325_bzzz_CHAMPION_DecrementStamina(/*P496_i_ChampionIndex, */rand.Next(4) + 4);
                goto T231_016;
            }
            T231_015:
            L0565_i_Damage = 0;
            //comment L0569_i_Outcome = C0_OUTCOME_KILLED_NO_CREATURES_IN_GROUP;
            F325_bzzz_CHAMPION_DecrementStamina(/*P496_i_ChampionIndex, */rand.Next(2) + 2);
            T231_016:
            //commented
            //F292_arzz_CHAMPION_DrawState(P496_i_ChampionIndex);
            //if (L0569_i_Outcome != C2_OUTCOME_KILLED_ALL_CREATURES_IN_GROUP)
            //{
            //    F209_xzzz_GROUP_ProcessEvents29to41(P499_i_MapX, P500_i_MapY, CM1_EVENT_CREATE_REACTION_EVENT_31_PARTY_IS_ADJACENT, 0);
            //}
            return L0565_i_Damage;
        }


        void F325_bzzz_CHAMPION_DecrementStamina(/*P672_i_ChampionIndex,*/ int P673_i_Decrement)
        {
            int L0988_i_Stamina;
            IEntity L0989_ps_Champion;

            L0989_ps_Champion = attackProvider; // &G407_s_Party.Champions[P672_i_ChampionIndex];
            var stamina = L0989_ps_Champion.GetProperty(StaminaPropertyFactory.Instance);
            if ((L0988_i_Stamina = (stamina.StoredValue -= P673_i_Decrement)) <= 0)
            {
                stamina.StoredValue = 0;// L0989_ps_Champion->CurrentStamina = 0;
                //TODO finish
                //F321_AA29_CHAMPION_AddPendingDamageAndWounds_GetDamage(P672_i_ChampionIndex, (-L0988_i_Stamina) >> 1, MASK0x0000_NO_WOUND, C0_ATTACK_NORMAL);
            }
            else
            {
                if (L0988_i_Stamina > stamina.Maxiumum)
                {
                    stamina.StoredValue = stamina.Maxiumum; ///L0989_ps_Champion->CurrentStamina = L0989_ps_Champion->MaximumStamina;
                }
            }
            //TODO finish right way
            //M08_SET(L0989_ps_Champion->Attributes, MASK0x0200_LOAD | MASK0x0100_STATISTICS);
        }


        void F304_apzz_CHAMPION_AddSkillExperience( /*P636_ui_ChampionIndex,*/ SkillInfo P637_ui_SkillIndex, int P638_ui_Experience)
        {
            int AP638_ui_SkillLevelAfter;
            int AP638_ui_ChampionColor;
            int A0915_ui_MapDifficulty;
            int A0915_ui_SkillLevelBefore;
            int A0915_ui_VitalityAmount;
            int A0915_ui_StaminaAmount;
            SkillInfo L0916_i_BaseSkillIndex;
            long L0917_l_Experience_Useless; /* BUG0_00 Useless code */
            ISkillFactory L0918_ps_Skill;
            IEntity L0919_ps_Champion;
            int L0920_i_MinorStatisticIncrease;
            int L0921_i_MajorStatisticIncrease;
            int L0922_i_BaseSkillLevel;


            //TODO finish
            //if ((P637_ui_SkillIndex >= C04_SKILL_SWING) && (P637_ui_SkillIndex <= C11_SKILL_SHOOT) && (G361_l_LastCreatureAttackTime < (G313_ul_GameTime - 150)))
            //{
            //    P638_ui_Experience >>= 1;
            //}

            if (P638_ui_Experience > 0)
            {
                //if (A0915_ui_MapDifficulty = G269_ps_CurrentMap->C.Difficulty)
                A0915_ui_MapDifficulty = attackProvider.Location.Tile.LevelIndex; //TODO make it right
                if (A0915_ui_MapDifficulty > 0)
                {
                    P638_ui_Experience *= A0915_ui_MapDifficulty;
                }
                L0919_ps_Champion = attackProvider; //&G407_s_Party.Champions[P636_ui_ChampionIndex];
                if (P637_ui_SkillIndex.IsHidden)// >= C04_SKILL_SWING)
                {
                    L0916_i_BaseSkillIndex = new SkillInfo(P637_ui_SkillIndex.BasicSkill); //(P637_ui_SkillIndex - C04_SKILL_SWING) >> 2;
                }
                else
                {
                    L0916_i_BaseSkillIndex = P637_ui_SkillIndex;
                }
                //L0917_l_Experience_Useless = L0918_ps_Skill->Experience; /* BUG0_00 Useless code. Use of uninitialized variable L0918_ps_Skill */
                //A0915_ui_SkillLevelBefore = F303_AA09_CHAMPION_GetSkillLevel(P636_ui_ChampionIndex, L0916_i_BaseSkillIndex | (MASK0x4000_IGNORE_OBJECT_MODIFIERS | MASK0x8000_IGNORE_TEMPORARY_EXPERIENCE));
                var param = new SkillInfo(L0916_i_BaseSkillIndex.Skill)
                {
                    IgnoreObjectModifiers = true,
                    IgnoreTemporaryExperience = true
                };
                A0915_ui_SkillLevelBefore = F303_AA09_CHAMPION_GetSkillLevel( /*P636_ui_ChampionIndex, */ param);
                //TODO finish
                //if ((P637_ui_SkillIndex >= C04_SKILL_SWING) && (G361_l_LastCreatureAttackTime > (G313_ul_GameTime - 25)))
                //{
                //    P638_ui_Experience <<= 1;
                //}
                L0918_ps_Skill = P637_ui_SkillIndex.Skill; // &L0919_ps_Champion->Skills[P637_ui_SkillIndex];
                L0918_ps_Skill.Experience += P638_ui_Experience;
                if (L0918_ps_Skill.TemporaryExperience < 32000)
                {
                    L0918_ps_Skill.TemporaryExperience += MathHelper.Clamp(P638_ui_Experience >> 3, 1, 100); //F026_a003_MAIN_GetBoundedValue(1, P638_ui_Experience >> 3, 100);
                }
                L0918_ps_Skill = L0916_i_BaseSkillIndex.Skill; //&L0919_ps_Champion->Skills[L0916_i_BaseSkillIndex];
                //if (P637_ui_SkillIndex >= C04_SKILL_SWING)
                if (P637_ui_SkillIndex.IsHidden)
                {
                    L0918_ps_Skill.Experience += P638_ui_Experience;
                }

                //AP638_ui_SkillLevelAfter = F303_AA09_CHAMPION_GetSkillLevel(P636_ui_ChampionIndex, L0916_i_BaseSkillIndex | (MASK0x4000_IGNORE_OBJECT_MODIFIERS | MASK0x8000_IGNORE_TEMPORARY_EXPERIENCE));
                var paramx = new SkillInfo(L0916_i_BaseSkillIndex.Skill)
                {
                    IgnoreObjectModifiers = true,
                    IgnoreTemporaryExperience = true
                };
                AP638_ui_SkillLevelAfter = F303_AA09_CHAMPION_GetSkillLevel( /*P636_ui_ChampionIndex,*/paramx);



                if (AP638_ui_SkillLevelAfter > A0915_ui_SkillLevelBefore)
                {
                    L0922_i_BaseSkillLevel = AP638_ui_SkillLevelAfter;
                    L0920_i_MinorStatisticIncrease = rand.Next(2);
                    L0921_i_MajorStatisticIncrease = 1 + rand.Next(2);
                    A0915_ui_VitalityAmount = rand.Next(2); /* For Priest skill, the amount is 0 or 1 for all skill levels */
                    if (L0916_i_BaseSkillIndex.Skill != PriestSkillFactory.Instance)//C02_SKILL_PRIEST)
                    {
                        A0915_ui_VitalityAmount &= AP638_ui_SkillLevelAfter; /* For non Priest skills the amount is 0 for even skill levels. The amount is 0 or 1 for odd skill levels */
                    }
                    //L0919_ps_Champion->Statistics[C4_STATISTIC_VITALITY][C0_MAXIMUM] += A0915_ui_VitalityAmount;
                    L0919_ps_Champion.GetProperty(VitalityPropertyFactory.Instance).Maxiumum += A0915_ui_VitalityAmount;
                    A0915_ui_StaminaAmount = L0919_ps_Champion.GetProperty(StaminaPropertyFactory.Instance).Maxiumum; // L0919_ps_Champion->MaximumStamina;
                    //L0919_ps_Champion->Statistics[C6_STATISTIC_ANTIFIRE][C0_MAXIMUM] += rand.Next(2) & ~AP638_ui_SkillLevelAfter; /* The amount is 0 for odd skill levels. The amount is 0 or 1 for even skill levels */
                    L0919_ps_Champion.GetProperty(AntiFirePropertyFactory.Instance).Maxiumum += rand.Next(2) & ~AP638_ui_SkillLevelAfter;
                    L0916_i_BaseSkillIndex.Skill.ModifySkill(attackProvider, ref A0915_ui_StaminaAmount, ref AP638_ui_SkillLevelAfter, L0921_i_MajorStatisticIncrease, L0920_i_MinorStatisticIncrease);
                    //switch (L0916_i_BaseSkillIndex)
                    //{
                    //    case C00_SKILL_FIGHTER:
                    //        A0915_ui_StaminaAmount >>= 4;
                    //        AP638_ui_SkillLevelAfter *= 3;
                    //        L0919_ps_Champion->Statistics[C1_STATISTIC_STRENGTH][C0_MAXIMUM] += L0921_i_MajorStatisticIncrease;
                    //        L0919_ps_Champion->Statistics[C2_STATISTIC_DEXTERITY][C0_MAXIMUM] += L0920_i_MinorStatisticIncrease;
                    //        break;
                    //    case C01_SKILL_NINJA:
                    //        A0915_ui_StaminaAmount /= 21;
                    //        AP638_ui_SkillLevelAfter <<= 1;
                    //        L0919_ps_Champion->Statistics[C1_STATISTIC_STRENGTH][C0_MAXIMUM] += L0920_i_MinorStatisticIncrease;
                    //        L0919_ps_Champion->Statistics[C2_STATISTIC_DEXTERITY][C0_MAXIMUM] += L0921_i_MajorStatisticIncrease;
                    //        break;
                    //    case C03_SKILL_WIZARD:
                    //        A0915_ui_StaminaAmount >>= 5;
                    //        L0919_ps_Champion->MaximumMana += AP638_ui_SkillLevelAfter + (AP638_ui_SkillLevelAfter >> 1);
                    //        L0919_ps_Champion->Statistics[C3_STATISTIC_WISDOM][C0_MAXIMUM] += L0921_i_MajorStatisticIncrease;
                    //        goto T304_016;
                    //    case C02_SKILL_PRIEST:
                    //        A0915_ui_StaminaAmount /= 25;
                    //        L0919_ps_Champion->MaximumMana += AP638_ui_SkillLevelAfter;
                    //        AP638_ui_SkillLevelAfter += (AP638_ui_SkillLevelAfter + 1) >> 1;
                    //        L0919_ps_Champion->Statistics[C3_STATISTIC_WISDOM][C0_MAXIMUM] += L0920_i_MinorStatisticIncrease;
                    //        T304_016:
                    //        if ((L0919_ps_Champion->MaximumMana += F024_aatz_MAIN_GetMinimumValue(M04_RANDOM(4), L0922_i_BaseSkillLevel - 1)) > 900)
                    //        {
                    //            L0919_ps_Champion->MaximumMana = 900;
                    //        }
                    //        L0919_ps_Champion->Statistics[C5_STATISTIC_ANTIMAGIC][C0_MAXIMUM] += M02_RANDOM(3);
                    //        break;
                    //}
                    //if ((L0919_ps_Champion->MaximumHealth += AP638_ui_SkillLevelAfter + M02_RANDOM((AP638_ui_SkillLevelAfter >> 1) + 1)) > 999)
                    var healthProperty = L0919_ps_Champion.GetProperty(HealthPropertyFactory.Instance);
                    healthProperty.Maxiumum += AP638_ui_SkillLevelAfter + rand.Next((AP638_ui_SkillLevelAfter >> 1) + 1);
                    if (healthProperty.Maxiumum > 999)
                    {
                        healthProperty.Maxiumum = 999; //L0919_ps_Champion->MaximumHealth = 999;
                    }


                    //if ((L0919_ps_Champion->MaximumStamina += A0915_ui_StaminaAmount + M02_RANDOM((A0915_ui_StaminaAmount >> 1) + 1)) > 9999)
                    var staminaProperty = L0919_ps_Champion.GetProperty(StaminaPropertyFactory.Instance);
                    staminaProperty.Maxiumum += A0915_ui_StaminaAmount + rand.Next((A0915_ui_StaminaAmount >> 1) + 1);
                    if (staminaProperty.Maxiumum > 9999)
                    {
                        staminaProperty.Maxiumum = 9999;
                    }
                    //TODO M08_SET(L0919_ps_Champion->Attributes, MASK0x0100_STATISTICS);
                    //                    F292_arzz_CHAMPION_DrawState(P636_ui_ChampionIndex);
                    //                    F051_AA19_TEXT_MESSAGEAREA_PrintLineFeed();
                    //                    F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(AP638_ui_ChampionColor = G046_auc_Graphic562_ChampionColor[P636_ui_ChampionIndex], L0919_ps_Champion->Name);
                    //# ifdef C09_COMPILE_DM10aEN_DM10bEN_DM11EN_DM12EN_CSB20EN_CSB21EN /* CHANGE4_00_LOCALIZATION Translation to German language */
                    //                    F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(AP638_ui_ChampionColor, " JUST GAINED A ");
                    //#endif
                    //# ifdef C21_COMPILE_DM12GE /* CHANGE4_00_LOCALIZATION Translation to German language */
                    //                    F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(AP638_ui_ChampionColor, " HAT SOEBEN ");
                    //#endif
                    //# ifdef C23_COMPILE_DM13aFR_DM13bFR /* CHANGE5_00_LOCALIZATION Translation to French language */
                    //                    F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(AP638_ui_ChampionColor, " VIENT DE DEVENIR ");
                    //#endif
                    //                    F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(AP638_ui_ChampionColor, G417_apc_BaseSkillNames[L0916_i_BaseSkillIndex]);
                    //# ifdef C09_COMPILE_DM10aEN_DM10bEN_DM11EN_DM12EN_CSB20EN_CSB21EN /* CHANGE4_00_LOCALIZATION Translation to German language */
                    //                    F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(AP638_ui_ChampionColor, " LEVEL!");
                    //#endif
                    //# ifdef C21_COMPILE_DM12GE /* CHANGE4_00_LOCALIZATION Translation to German language */
                    //                    F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(AP638_ui_ChampionColor, " STUFE ERREICHT!");
                    //#endif
                    //# ifdef C23_COMPILE_DM13aFR_DM13bFR /* CHANGE5_00_LOCALIZATION Translation to French language */
                    //                    F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(AP638_ui_ChampionColor, "!");
                    //#endif
                }
            }
        }


        int F311_wzzz_CHAMPION_GetDexterity()
        {
            int L0934_i_Dexterity;

            L0934_i_Dexterity = rand.Next(8) + attackProvider.GetProperty(DextrityPropertyFactory.Instance).CurrentValue;// P649_ps_Champion->Statistics[C2_STATISTIC_DEXTERITY][C1_CURRENT];
            L0934_i_Dexterity -= (/*(long)*/(L0934_i_Dexterity >> 1) * /*(long)*/attackProvider.GetProperty(LoadPropertyFactory.Instance).BaseValue/*P649_ps_Champion->Load*/) / F309_awzz_CHAMPION_GetMaximumLoad(/*P649_ps_Champion*/);
            //TODO party sleeping
            //if (G300_B_PartyIsSleeping) {
            //        L0934_i_Dexterity >>= 1;
            //}
            return MathHelper.Clamp(L0934_i_Dexterity >> 1, 1 + rand.Next(8), 100 - rand.Next(8));// F026_a003_MAIN_GetBoundedValue(1 + M03_RANDOM(8), L0934_i_Dexterity >> 1, 100 - M03_RANDOM(8));
        }

        bool F308_vzzz_CHAMPION_IsLucky(int P646_ui_Percentage)
        {
            if (rand.Next(2) != 0 && (rand.Next(100) > P646_ui_Percentage))
                return true;

            var luckProperty = attackProvider.GetProperty(LuckPropertyFactory.Instance);
            bool AP646_ui_IsLucky = rand.Next(luckProperty.CurrentValue) > P646_ui_Percentage;
            luckProperty.BaseValue += AP646_ui_IsLucky ? -2 : 2;
            return AP646_ui_IsLucky;
        }

        int F306_xxxx_CHAMPION_GetStaminaAdjustedValue( /*P640_ps_Champion,*/ int P641_i_Value)
        {
            int L0925_i_CurrentStamina;
            int L0926_i_HalfMaximumStamina;

            //if ((L0925_i_CurrentStamina = P640_ps_Champion->CurrentStamina) < (L0926_i_HalfMaximumStamina = P640_ps_Champion->MaximumStamina >> 1))
            var staminaProperty = attackProvider.GetProperty(StaminaPropertyFactory.Instance);
            L0925_i_CurrentStamina = staminaProperty.CurrentValue;
            L0926_i_HalfMaximumStamina = staminaProperty.Maxiumum >> 1;
            if (L0925_i_CurrentStamina < L0926_i_HalfMaximumStamina)
            {
                return (P641_i_Value >>= 1) + (int)(((long)P641_i_Value * (long)L0925_i_CurrentStamina) / L0926_i_HalfMaximumStamina);
            }
            return P641_i_Value;
        }

        int F303_AA09_CHAMPION_GetSkillLevel(/*P634_i_ChampionIndex ,*/ SkillInfo P635_ui_SkillIndex)
        {
            long L0907_l_Experience;
            int L0908_i_SkillLevel;
            int L0909_i_NeckIconIndex;
            int L0910_i_ActionHandIconIndex;
            ISkillFactory L0911_ps_Skill;
            IEntity L0912_ps_Champion;
            bool L0913_B_IgnoreTemporaryExperience;
            bool L0914_B_IgnoreObjectModifiers;

            //TODO party sleeping
            //if (G300_B_PartyIsSleeping)
            //{
            //    return 1;
            //}

            L0913_B_IgnoreTemporaryExperience = P635_ui_SkillIndex.IgnoreTemporaryExperience;//  M07_GET(P635_ui_SkillIndex, MASK0x8000_IGNORE_TEMPORARY_EXPERIENCE);
            L0914_B_IgnoreObjectModifiers = P635_ui_SkillIndex.IgnoreObjectModifiers;// M07_GET(P635_ui_SkillIndex, MASK0x4000_IGNORE_OBJECT_MODIFIERS);
            P635_ui_SkillIndex.IgnoreObjectModifiers = P635_ui_SkillIndex.IgnoreTemporaryExperience = false; //M09_CLEAR(P635_ui_SkillIndex, MASK0x8000_IGNORE_TEMPORARY_EXPERIENCE | MASK0x4000_IGNORE_OBJECT_MODIFIERS);
            L0912_ps_Champion = attackProvider;// &G407_s_Party.Champions[P634_i_ChampionIndex];
            L0911_ps_Skill = P635_ui_SkillIndex.Skill;// &L0912_ps_Champion->Skills[P635_ui_SkillIndex];
            L0907_l_Experience = L0911_ps_Skill.Experience;
            if (!L0913_B_IgnoreTemporaryExperience)
            {
                L0907_l_Experience += L0911_ps_Skill.TemporaryExperience;
            }
            //if (P635_ui_SkillIndex > C03_SKILL_WIZARD)
            if (P635_ui_SkillIndex.IsHidden)
            {
                /* Hidden skill */
                L0911_ps_Skill = P635_ui_SkillIndex.BasicSkill;  //&L0912_ps_Champion->Skills[(P635_ui_SkillIndex - C04_SKILL_SWING) >> 2]; //divide hidden skills to four groups
                L0907_l_Experience += L0911_ps_Skill.Experience; /* Add experience in the base skill */
                if (!L0913_B_IgnoreTemporaryExperience)
                {
                    L0907_l_Experience += L0911_ps_Skill.TemporaryExperience;
                }
                L0907_l_Experience >>= 1; /* Halve experience to get average of base skill + hidden skill experience */
            }
            L0908_i_SkillLevel = 1;
            while (L0907_l_Experience >= 500)
            {
                L0907_l_Experience >>= 1;
                L0908_i_SkillLevel++;
            }

            //TODO object modifers
            //            if (!L0914_B_IgnoreObjectModifiers)
            //            {
            //                if ((L0910_i_ActionHandIconIndex = F033_aaaz_OBJECT_GetIconIndex(L0912_ps_Champion->Slots[C01_SLOT_ACTION_HAND])) == C027_ICON_WEAPON_THE_FIRESTAFF)
            //                {
            //                    L0908_i_SkillLevel++;
            //                }
            //                else
            //                {
            //                    if (L0910_i_ActionHandIconIndex == C028_ICON_WEAPON_THE_FIRESTAFF_COMPLETE)
            //                    {
            //                        L0908_i_SkillLevel += 2;
            //    //#ifdef C02_COMPILE_DM10aEN_DM10bEN_DM11EN /* CHANGE3_09_FIX The skill modifier of the Firestaff is not cumulative with other modifiers */
            //    //                    }
            //    //                    else
            //    //                    {
            //    //                        /* BUG0_40 If a champion has The Firestaff in the action hand then skill modifiers from other objects (Pendant Feral, Ekkhard Cross, Gem of Ages and Moonstone) are ignored. Wrong code causes the skill modifier of the Firesatff not to be cumulative with other modifiers */
            //    //#endif
            //    //#ifdef C19_COMPILE_DM12EN_DM12GE_DM13aFR_DM13bFR_CSB20EN_CSB21EN /* CHANGE3_09_FIX The skill modifier of the Firesatff is cumulative with other modifiers */
            //                    }
            //                }
            ////#endif
            //                L0909_i_NeckIconIndex = F033_aaaz_OBJECT_GetIconIndex(L0912_ps_Champion->Slots[C10_SLOT_NECK]);
            //                switch (P635_ui_SkillIndex)
            //                {
            //                    case C03_SKILL_WIZARD:
            //                        if (L0909_i_NeckIconIndex == C124_ICON_JUNK_PENDANT_FERAL)
            //                        {
            //                            L0908_i_SkillLevel += 1;
            //                        }
            //                        break;
            //                    case C15_SKILL_DEFEND:
            //                        if (L0909_i_NeckIconIndex == C121_ICON_JUNK_EKKHARD_CROSS)
            //                        {
            //                            L0908_i_SkillLevel += 1;
            //                        }
            //                        break;
            //                    case C13_SKILL_HEAL:
            //                        if ((L0909_i_NeckIconIndex == C120_ICON_JUNK_GEM_OF_AGES) || (L0910_i_ActionHandIconIndex == C066_ICON_WEAPON_SCEPTRE_OF_LYF))
            //                        {
            //                            /* The skill modifiers of these two objects are not cumulative */
            //                            L0908_i_SkillLevel += 1;
            //                        }
            //                        break;
            //                    case C14_SKILL_INFLUENCE:
            //                        if (L0909_i_NeckIconIndex == C122_ICON_JUNK_MOONSTONE)
            //                        {
            //                            L0908_i_SkillLevel += 1;
            //                        }
            //                }
            ////#ifdef C02_COMPILE_DM10aEN_DM10bEN_DM11EN /* CHANGE3_09_FIX */
            ////            }
            ////        }
            ////#endif
            //    }
            return L0908_i_SkillLevel;
        }

        int F309_awzz_CHAMPION_GetMaximumLoad( /*P647_ps_Champion*/)

        {
            int L0929_ui_MaximumLoad;
            int L0930_i_Wounds;

            L0929_ui_MaximumLoad = (attackProvider.GetProperty(StrengthPropertyFactory.Instance).CurrentValue << 3) + 100;//  (P647_ps_Champion->Statistics[C1_STATISTIC_STRENGTH][C1_CURRENT] << 3) + 100;
            L0929_ui_MaximumLoad = F306_xxxx_CHAMPION_GetStaminaAdjustedValue(/*P647_ps_Champion,*/ L0929_ui_MaximumLoad);
            //if (L0930_i_Wounds = P647_ps_Champion->Wounds)
            if (attackProvider.Body.BodyParts.Any(b => b.IsWound))
            {
                L0929_ui_MaximumLoad -= L0929_ui_MaximumLoad >> (attackProvider.Body.BodyParts.First(b => b.Type == LegsStorageType.Instance).IsWound ? 2 : 3);// (M07_GET(L0930_i_Wounds, MASK0x0010_LEGS) ? 2 : 3);
            }
            //TODO should be done somewhat else
            //if (F033_aaaz_OBJECT_GetIconIndex(P647_ps_Champion->Slots[C05_SLOT_FEET]) == C119_ICON_ARMOUR_ELVEN_BOOTS)
            //{
            //    L0929_ui_MaximumLoad += L0929_ui_MaximumLoad >> 4;
            //}
            L0929_ui_MaximumLoad += 9;
            L0929_ui_MaximumLoad -= L0929_ui_MaximumLoad % 10; /* Round the value to the next multiple of 10 */
            return L0929_ui_MaximumLoad;
        }

        int F312_xzzz_CHAMPION_GetStrength(IStorageType P651_i_SlotIndex)
        {
            int L0935_i_Strength;
            int A0936_ui_ObjectWeight;
            int A0936_ui_SkillLevel;
            int A0937_ui_OneSixteenthMaximumLoad;
            WeaponClass A0937_ui_Class;
            IGrabableItem L0938_T_Thing;
            IEntity L0939_ps_Champion;
            Weapon L0940_ps_WeaponInfo;
            int L0941_i_LoadThreshold;


            L0939_ps_Champion = attackProvider;// &G407_s_Party.Champions[P650_i_ChampionIndex];
            L0935_i_Strength = rand.Next(16) + L0939_ps_Champion.GetProperty(StrengthPropertyFactory.Instance).CurrentValue;//M03_RANDOM(16) + L0939_ps_Champion->Statistics[C1_STATISTIC_STRENGTH][C1_CURRENT];
            L0938_T_Thing = L0939_ps_Champion.Body.GetStorage(P651_i_SlotIndex).Storage.First();// L0939_ps_Champion->Slots[P651_i_SlotIndex];
            //if ((A0936_ui_ObjectWeight = F140_yzzz_DUNGEON_GetObjectWeight(L0938_T_Thing)) <= (A0937_ui_OneSixteenthMaximumLoad = F309_awzz_CHAMPION_GetMaximumLoad(L0939_ps_Champion) >> 4))
            A0936_ui_ObjectWeight = L0938_T_Thing.Weight;
            A0937_ui_OneSixteenthMaximumLoad = F309_awzz_CHAMPION_GetMaximumLoad() >> 4;
            if (A0936_ui_ObjectWeight <= A0937_ui_OneSixteenthMaximumLoad)
            {
                L0935_i_Strength += A0936_ui_ObjectWeight - 12;
            }
            else
            {
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
                L0940_ps_WeaponInfo = L0938_T_Thing as Weapon;  //F158_ayzz_DUNGEON_GetWeaponInfo(L0938_T_Thing);
                //#endif
                L0935_i_Strength += L0940_ps_WeaponInfo.Strength;
                A0936_ui_SkillLevel = 0;
                A0937_ui_Class = L0940_ps_WeaponInfo.Class;
                if ((A0937_ui_Class == WeaponClass.C000_CLASS_SWING_WEAPON) || (A0937_ui_Class == WeaponClass.C002_CLASS_DAGGER_AND_AXES))
                {
                    A0936_ui_SkillLevel = F303_AA09_CHAMPION_GetSkillLevel(new SkillInfo(SwingSkillFactory.Instance));// P650_i_ChampionIndex, C04_SKILL_SWING);
                }
                if ((A0937_ui_Class != WeaponClass.C000_CLASS_SWING_WEAPON) && (A0937_ui_Class < WeaponClass.C016_CLASS_FIRST_BOW))
                {
                    A0936_ui_SkillLevel += F303_AA09_CHAMPION_GetSkillLevel(new SkillInfo(ThrowSkillFactory.Instance));//P650_i_ChampionIndex, C10_SKILL_THROW);
                }
                if ((A0937_ui_Class >= WeaponClass.C016_CLASS_FIRST_BOW) && (A0937_ui_Class < WeaponClass.C112_CLASS_FIRST_MAGIC_WEAPON))
                {
                    A0936_ui_SkillLevel += F303_AA09_CHAMPION_GetSkillLevel(new SkillInfo(ShootSkillFactory.Instance));//P650_i_ChampionIndex, C11_SKILL_SHOOT);
                }
                L0935_i_Strength += A0936_ui_SkillLevel << 1;
            }
            L0935_i_Strength = F306_xxxx_CHAMPION_GetStaminaAdjustedValue(/*L0939_ps_Champion,*/ L0935_i_Strength);
            //if (M07_GET(L0939_ps_Champion->Wounds, (P651_i_SlotIndex == C00_SLOT_READY_HAND) ? MASK0x0001_READY_HAND : MASK0x0002_ACTION_HAND))
            if (attackProvider.Body.GetBodyStoratge(P651_i_SlotIndex).IsWound)
            {
                L0935_i_Strength >>= 1;
            }
            //return F026_a003_MAIN_GetBoundedValue(0, L0935_i_Strength >> 1, 100);
            return MathHelper.Clamp(L0935_i_Strength >> 1, 0, 100);
        }


    }
}