using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.CreatureSpecific;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public class MeleeAttack : HumanAttack
    {
        //case C024_ACTION_DISRUPT:
        //case C016_ACTION_JAB:
        //case C017_ACTION_PARRY:
        //case C014_ACTION_STAB:
        //case C009_ACTION_STAB:
        //case C031_ACTION_STUN:
        //case C015_ACTION_THRUST:
        //case C025_ACTION_MELEE:
        //case C028_ACTION_SLASH:
        //case C029_ACTION_CLEAVE:
        //case C006_ACTION_PUNCH:

        public MeleeAttack(HumanActionFactoryBase factoryBase, ILiveEntity attackProvider) : base(factoryBase, attackProvider) { }
        protected override void PerformAttack(MapDirection direction, ref int delay)
        {
            if (F402_xxxx_MENUS_IsMeleeActionPerformed(direction))
                delay >>= 1;
        }

        protected bool F402_xxxx_MENUS_IsMeleeActionPerformed(MapDirection direction)
        {
            var enemy = GetAccesibleEnemies(direction); //aka ismeleeActionPerformed
            if (enemy != null)
            {

                if (false) //TODO (P775_i_ActionIndex == C024_ACTION_DISRUPT) && !M07_GET(F144_amzz_DUNGEON_GetCreatureAttributes(G517_T_ActionTargetGroupThing), MASK0x0040_NON_MATERIAL))
                {
                }
                else
                {
                    ActionProbabilityInfo A1237_i_ActionHitProbability = new ActionProbabilityInfo(factoryBase.HitProbability); //G493_auc_Graphic560_ActionHitProbability[P775_i_ActionIndex];
                    int A1236_ui_ActionDamageFactor = factoryBase.Damage;  //G492_auc_Graphic560_ActionDamageFactor[P775_i_ActionIndex];
                    A1237_i_ActionHitProbability.HitNonmaterial = attackProvider.GetProperty(PropertyFactory<NonMaterialProperty>.Instance).MaxValue > 0;
                    //if ((F033_aaaz_OBJECT_GetIconIndex(P774_ps_Champion->Slots[C01_SLOT_ACTION_HAND]) == C040_ICON_WEAPON_VORPAL_BLADE) || (P775_i_ActionIndex == C024_ACTION_DISRUPT))
                    //{
                    //    M08_SET(A1237_i_ActionHitProbability, MASK0x8000_HIT_NON_MATERIAL_CREATURES);
                    //}
                    F231_izzz_GROUP_GetMeleeActionDamage(enemy, A1237_i_ActionHitProbability, A1236_ui_ActionDamageFactor/*, P778_i_SkillIndex*/);
                    return true;
                }
            }
            return false;
        }



        protected int F231_izzz_GROUP_GetMeleeActionDamage(ILiveEntity enemy, ActionProbabilityInfo P501_i_ActionHitProbability, int P502_ui_ActionDamageFactor)
        {
            int L0565_i_Damage;
            int L0568_i_Defense = 0;
            bool L0570_B_ActionHitsNonMaterialCreatures;

            int L0567_i_DoubledMapDifficulty = attackProvider.Location.Tile.Level.Difficulty;
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
                 (rand.Next(4) == 0) || (F308_vzzz_CHAMPION_IsLucky(/*P495_ps_Champion,*/attackProvider, 75 - P501_i_ActionHitProbability.Value))))
            {
                //if (( //condition added onto fllowing block and to jumping locaiton t231_009
                L0565_i_Damage = F312_xzzz_CHAMPION_GetStrength(attackProvider, /*P496_i_ChampionIndex, C01_SLOT_ACTION_HAND,*/ ActionHandStorageType.Instance);
                //{
                //goto T231_009;/
                //}
                //else//

                if (L0565_i_Damage > 0)
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
                if ((L0566_i_Damage = L0565_i_Damage) <= 0 || (L0566_i_Damage = L0565_i_Damage = rand.Next(32) + L0565_i_Damage - L0568_i_Defense) <= 1)
                {
                    //T231_009:
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
                if (rand.Next(64) < attackProvider.GetSkill(factoryBase.SkillIndex).SkillLevel)//  F303_AA09_CHAMPION_GetSkillLevel(/*P496_i_ChampionIndex,*/ P503_i_SkillIndex))
                {
                    L0565_i_Damage += L0565_i_Damage + 10;
                }
                //L0569_i_Outcome = F190_zzzz_GROUP_GetDamageCreatureOutcome(P497_ps_Group, P498_i_CreatureIndex, P499_i_MapX, P500_i_MapY, L0565_i_Damage, true);
                enemy.GetProperty(PropertyFactory<HealthProperty>.Instance).Value -= L0565_i_Damage;
                //F304_apzz_CHAMPION_AddSkillExperience(/*P496_i_ChampionIndex,*/ P503_i_SkillIndex, (L0565_i_Damage * L0572_ps_CreatureInfo.Experience /*M58_EXPERIENCE(L0572_ps_CreatureInfo->Properties)*/ >> 4) + 3);
                var experienceGain = enemy.GetProperty(PropertyFactory<ExperienceProperty>.Instance).Value;
                attackProvider.GetSkill(factoryBase.SkillIndex).AddExperience((L0565_i_Damage * experienceGain >> 4) + 3);

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

    }
}