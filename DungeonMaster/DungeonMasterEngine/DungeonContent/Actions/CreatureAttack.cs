using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.ChampionSpecific;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actions
{
    public class CreatureAttackFactoryBase : IActionFactory
    {
        public IAction CreateAction(ILiveEntity actionProvider)
        {
            throw new NotImplementedException();
        }
    }

    public class CreatureAttack : AttackBase<CreatureAttackFactoryBase> 
    {

        //#define C00_SLOT_READY_HAND        0
        //#define C01_SLOT_ACTION_HAND       1
        //#define C02_SLOT_HEAD              2
        //#define C03_SLOT_TORSO             3
        //#define C04_SLOT_LEGS              4
        //#define C05_SLOT_FEET              5
        // todo values http://dmweb.free.fr/?q=node/1398#toc35 => resitance to injuries multiplers

        public Creature AttackProvider { get; }

        public CreatureAttack(Creature attackProvider) : base(null)
        {
            this.AttackProvider = attackProvider;
        }


        public override int ApplyAttack(MapDirection direction)
        {
            var tile = AttackProvider.Location.Tile.Neighbors.GetTile(direction);

            if (tile != null)
            {
                var enemies = tile.LayoutManager.Entities.Where(x => AttackProvider.RelationManager.IsEnemy(x.RelationManager.RelationToken)).ToArray();
                if (enemies.Length > 0)
                {

                    ILiveEntity enemy;
                    if (AttackProvider.Factory.AttackAny)
                        enemy = tile.LayoutManager.Entities.ElementAt(rand.Next(4) % enemies.Length);
                    else
                        enemy = enemies.MinObj(c => Vector3.Distance(c.Position, AttackProvider.Position));


                    F207_xxxx_GROUP_IsCreatureAttacking(enemy);
                }
            }
            return 0;
        }


        protected virtual bool F207_xxxx_GROUP_IsCreatureAttacking(ILiveEntity enemy)
        {
            CreatureFactory L0441_ps_CreatureInfo = AttackProvider.Factory;

            int A0440_i_Damage = F230_ezzz_GROUP_GetChampionDamage(enemy) + 1;

            //Champion L0442_ps_Champion = enemy; 
            //if (A0440_i_Damage > L0442_ps_Champion->MaximumDamageReceived)
            //{
            //    L0442_ps_Champion->MaximumDamageReceived = A0440_i_Damage;
            //    L0442_ps_Champion->DirectionMaximumDamageReceived = M18_OPPOSITE(L0438_ui_PrimaryDirectionToParty);
            //}
            //if (A0440_i_AttackSoundOrdinal = L0441_ps_CreatureInfo->AttackSoundOrdinal)
            //{
            //    //TODO sound F064_aadz_SOUND_RequestPlay_COPYPROTECTIOND(G244_auc_Graphic559_CreatureAttackSounds[--A0440_i_AttackSoundOrdinal], P422_i_MapX, P423_i_MapY, C01_MODE_PLAY_IF_PRIORITIZED);
            //}
            return true;
        }


        int F230_ezzz_GROUP_GetChampionDamage( /*group P493_ps_Group, int */ ILiveEntity P494_i_ChampionIndex)
        {
            var champion = P494_i_ChampionIndex; //&G407_s_Party.Champions[P494_i_ChampionIndex];
            //if (P494_i_ChampionIndex >= G305_ui_PartyChampionCount)
            //{
            //    return 0;
            //}

            //if (!champion->CurrentHealth)
            //{
            //    return 0;
            //}

            //TODO sleeping
            //if (G300_B_PartyIsSleeping)
            //{
            //    F314_gzzz_CHAMPION_WakeUp();
            //}

            int L0563_i_DoubledMapDifficulty = AttackProvider.Location.Tile.Level.Difficulty << 1;
            CreatureFactory L0564_s_CreatureInfo = AttackProvider.Factory; //G243_as_Graphic559_CreatureInfo[P493_ps_Group->Type];

            //F304_apzz_CHAMPION_AddSkillExperience(P494_i_ChampionIndex, C07_SKILL_PARRY, M58_EXPERIENCE(L0564_s_CreatureInfo.Properties));
            champion.GetSkill(SkillFactory<ParrySkill>.Instance).AddExperience(L0564_s_CreatureInfo.Experience);

            bool partySleeping = false; //TODO
            bool dextrityTest = (champion.GetProperty(PropertyFactory<DextrityProperty>.Instance).Value < rand.Next(32) +/*L0564_s_CreatureInfo.Dexterity*/
                AttackProvider.GetProperty(PropertyFactory<DextrityProperty>.Instance).Value + L0563_i_DoubledMapDifficulty - 16) || rand.Next(4) == 0;
            bool luckTest = !F308_vzzz_CHAMPION_IsLucky(champion, 60);


            if (partySleeping || (dextrityTest && luckTest))
            {
                IStorageType A0561_ui_AllowedWound = GetAllowedWounds(L0564_s_CreatureInfo);

                int A0558_i_Attack;
                if ((A0558_i_Attack = (rand.Next(16) + L0564_s_CreatureInfo.Attack + L0563_i_DoubledMapDifficulty) -
                    (champion.GetSkill(SkillFactory<ParrySkill>.Instance).SkillLevel << 1)) <= 1)
                {
                    if (rand.Next(2) > 0)
                        return 0;

                    A0558_i_Attack = rand.Next(4) + 2;
                }

                A0558_i_Attack >>= 1;
                A0558_i_Attack += rand.Next(A0558_i_Attack) + rand.Next(4);
                A0558_i_Attack += rand.Next(A0558_i_Attack);
                A0558_i_Attack >>= 2;
                A0558_i_Attack += rand.Next(4) + 1;
                if (rand.Next(2) > 0)
                {
                    A0558_i_Attack -= rand.Next((A0558_i_Attack >> 1) + 1) - 1;
                }
                int A0558_i_Damage;
                if ((A0558_i_Damage = F321_AA29_CHAMPION_AddPendingDamageAndWounds_GetDamage(P494_i_ChampionIndex, A0558_i_Attack, new[] { A0561_ui_AllowedWound }, L0564_s_CreatureInfo.AttackType)) > 0)
                {
                    //TODO sounds F064_aadz_SOUND_RequestPlay_COPYPROTECTIOND(C09_SOUND_CHAMPION_0_DAMAGED + P494_i_ChampionIndex, G306_i_PartyMapX, G307_i_PartyMapY, C02_MODE_PLAY_ONE_TICK_LATER);
                    int A0559_ui_PoisonAttack;
                    if ((A0559_ui_PoisonAttack = L0564_s_CreatureInfo.PoisonAttack) > 0 && rand.Next(2) > 0 && ((A0559_ui_PoisonAttack = F307_fzzz_CHAMPION_GetStatisticAdjustedAttack(champion, PropertyFactory<VitalityProperty>.Instance, A0559_ui_PoisonAttack)) >= 0))
                    {
                        F322_lzzz_CHAMPION_Poison(P494_i_ChampionIndex, A0559_ui_PoisonAttack);
                    }
                    return A0558_i_Damage;
                }
            }
            return 0;
        }

        private static IStorageType GetAllowedWounds(CreatureFactory L0564_s_CreatureInfo)
        {
            int A0559_ui_WoundTest;
            if (((A0559_ui_WoundTest = rand.Next(65536)) & 0x0070) > 0)
            {
                A0559_ui_WoundTest &= 0x000F;
                int A0561_ui_WoundProbabilityIndex = 0;
                foreach (var woundValue in new int[]
                {
                        L0564_s_CreatureInfo.WoundFeet,
                        L0564_s_CreatureInfo.WoundLegs,
                        L0564_s_CreatureInfo.WoundTorso,
                        L0564_s_CreatureInfo.WoundHead
                })
                {
                    if (A0559_ui_WoundTest > woundValue)
                    {
                        A0561_ui_WoundProbabilityIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
                //A0561_ui_AllowedWound = G024_ac_Graphic562_WoundProbabilityIndexToWoundMask[A0561_ui_WoundProbabilityIndex];
                switch (A0561_ui_WoundProbabilityIndex - 1)
                {
                    case -1:
                        return null;
                    case 0:
                        return FeetsStorageType.Instance;
                    case 1:
                        return LegsStorageType.Instance;
                    case 2:
                        return TorsoStorageType.Instance;
                    case 3:
                        return HeadStorageType.Instance;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                if ((A0559_ui_WoundTest & 0x0001) == 0)/* 0 (Ready hand) or 1 (action hand) */
                    return ReadyHandStorageType.Instance;
                else
                    return ActionHandStorageType.Instance;
            }
        }

        int F307_fzzz_CHAMPION_GetStatisticAdjustedAttack(ILiveEntity P642_ps_Champion, IPropertyFactory P643_ui_StatisticIndex, int P644_ui_Attack)
        {
            int L0927_i_Factor;

            if ((L0927_i_Factor = 170 - P642_ps_Champion.GetProperty(P643_ui_StatisticIndex).Value) < 16) //[C1_CURRENT]
            {
                /* BUG0_41 The Antifire and Antimagic statistics are completely ignored. The Vitality statistic is ignored against poison and to determine the probability of being wounded. Vitality is still used normally to compute the defense against wounds and the speed of health regeneration. A bug in the Megamax C compiler produces wrong machine code for this statement. It always returns 0 for the current statistic value so that L0927_i_Factor = 170 in all cases */
                return P644_ui_Attack >> 3;
            }
            return F030_aaaW_MAIN_GetScaledProduct(P644_ui_Attack, 7, L0927_i_Factor);
        }

        int F321_AA29_CHAMPION_AddPendingDamageAndWounds_GetDamage(ILiveEntity P662_i_ChampionIndex, int P663_i_Attack, ICollection<IStorageType> P664_i_AllowedWounds, CreatureAttackType P665_i_AttackType)
        {
            int L0977_ui_Defense = 0;
            int L0978_ui_WoundCount = 0;

            //if ((P662_i_ChampionIndex == CM1_CHAMPION_NONE) || (M00_INDEX_TO_ORDINAL(P662_i_ChampionIndex) == G299_ui_CandidateChampionOrdinal || G302_B_GameWon))
            //{
            //    /* BUG0_00 Useless code. This condition is never true */
            //    return; /* BUG0_01 Coding error without consequence. Undefined return value. No consequence because this code is never executed */
            //}

            if (P663_i_Attack <= 0)
            {
                return 0;
            }

            var L0979_ps_Champion = P662_i_ChampionIndex; //&G407_s_Party.Champions[P662_i_ChampionIndex];
            if (L0979_ps_Champion.GetProperty(PropertyFactory<HealthProperty>.Instance).Value == 0)
                return 0;

            if (P665_i_AttackType != CreatureAttackType.C0_ATTACK_NORMAL)
            {
                //TODO add additional defense modifiers to DefensProperty
                foreach (var bodyPart in L0979_ps_Champion.Body.BodyParts)
                //for (L0978_ui_WoundCount = 0, A0976_i_WoundIndex = C00_SLOT_READY_HAND, L0977_ui_Defense = 0; A0976_i_WoundIndex <= C05_SLOT_FEET; A0976_i_WoundIndex++)
                {
                    if (P664_i_AllowedWounds.Contains(bodyPart.Type))
                    //if (P664_i_AllowedWounds & (1 << A0976_i_WoundIndex))
                    {
                        L0978_ui_WoundCount++;
                        L0977_ui_Defense += F313_xxxx_CHAMPION_GetWoundDefense(P662_i_ChampionIndex, new WoundInfo(P665_i_AttackType == CreatureAttackType.C4_ATTACK_SHARP, bodyPart.Type));
                        //A0976_i_WoundIndex | ((P665_i_AttackType == C4_ATTACK_SHARP) ? MASK0x8000_USE_SHARP_DEFENSE : MASK0x0000_DO_NOT_USE_SHARP_DEFENSE));
                    }
                }

                if (L0978_ui_WoundCount > 0)
                {
                    L0977_ui_Defense /= L0978_ui_WoundCount;
                }

                switch (P665_i_AttackType)
                {
                    case CreatureAttackType.C6_ATTACK_PSYCHIC:
                        int A0976_i_WisdomFactor;
                        //->Statistics[C3_STATISTIC_WISDOM][C1_CURRENT]) <= 0)
                        if ((A0976_i_WisdomFactor = 115 - L0979_ps_Champion.GetProperty(PropertyFactory<WisdomProperty>.Instance).Value) <= 0)
                        {
                            P663_i_Attack = 0;
                        }
                        else
                        {
                            P663_i_Attack = F030_aaaW_MAIN_GetScaledProduct(P663_i_Attack, 6, A0976_i_WisdomFactor);
                        }
                        goto T321_024;
                    case CreatureAttackType.C5_ATTACK_MAGIC:
                        P663_i_Attack = F307_fzzz_CHAMPION_GetStatisticAdjustedAttack(L0979_ps_Champion, PropertyFactory<AntiMagicProperty>.Instance, P663_i_Attack);
                        P663_i_Attack -= L0979_ps_Champion.GetProperty(PropertyFactory<SpellShieldDefenseProperty>.Instance).Value;// G407_s_Party.SpellShieldDefense;
                        goto T321_024;
                    case CreatureAttackType.C1_ATTACK_FIRE:
                        P663_i_Attack = F307_fzzz_CHAMPION_GetStatisticAdjustedAttack(L0979_ps_Champion, PropertyFactory<AntiFireProperty>.Instance, P663_i_Attack);
                        P663_i_Attack -= L0979_ps_Champion.GetProperty(PropertyFactory<FireShieldDefenseProperty>.Instance).Value; //G407_s_Party.FireShieldDefense;
                        break;
                    case CreatureAttackType.C2_ATTACK_SELF:
                        L0977_ui_Defense >>= 1;
                        break;
                    case CreatureAttackType.C3_ATTACK_BLUNT:
                    case CreatureAttackType.C4_ATTACK_SHARP:
                    case CreatureAttackType.C7_ATTACK_LIGHTNING:
                        break;
                }

                P663_i_Attack = F030_aaaW_MAIN_GetScaledProduct(P663_i_Attack, 6, 130 - L0977_ui_Defense); /* BUG0_44 A champion may take much more damage than expected after a Black Flame attack or an impact with a Fireball projectile. If the party has a fire shield defense value higher than the fire attack value then the resulting intermediary attack value is negative and damage should be 0. However, the negative value is still used for further computations and the result may be a very high positive attack value which may kill a champion. This can occur only for ATTACK_FIRE and if P663_i_Attack is negative before calling F030_aaaW_MAIN_GetScaledProduct */
                T321_024:
                if (P663_i_Attack <= 0)
                {
                    return 0;
                }

                int A0976_i_AdjustedAttack = F307_fzzz_CHAMPION_GetStatisticAdjustedAttack(L0979_ps_Champion, PropertyFactory<VitalityProperty>.Instance, rand.Next(128) + 10);
                if (P663_i_Attack > A0976_i_AdjustedAttack)
                {
                    /* BUG0_45 This bug is not perceptible because of BUG0_41 that ignores Vitality while determining the probability of being wounded. However if it was fixed, the behavior would be the opposite of what it should: the higher the vitality of a champion, the lower the result of F307_fzzz_CHAMPION_GetStatisticAdjustedAttack and the more likely the champion could get wounded (because of more iterations in the loop below) */
                    do
                    {
                        //M08_SET(G410_ai_ChampionPendingWounds[P662_i_ChampionIndex], (1 << rand.Next(8)) & P664_i_AllowedWounds);
                        var bodyPart = L0979_ps_Champion.Body.BodyParts.ElementAtOrDefault(rand.Next(8));
                        if (bodyPart != null && P664_i_AllowedWounds.Contains(bodyPart.Type))
                        {
                            bodyPart.IsWound = true;
                        }
                    }
                    while ((P663_i_Attack > (A0976_i_AdjustedAttack <<= 1)) && A0976_i_AdjustedAttack > 0);
                }

                //TODO wake up 
                //if (G300_B_PartyIsSleeping)
                //{
                //    F314_gzzz_CHAMPION_WakeUp();
                //}
            }

            //G409_ai_ChampionPendingDamage[P662_i_ChampionIndex] += P663_i_Attack; 
            L0979_ps_Champion.GetProperty(PropertyFactory<HealthProperty>.Instance).Value -= P663_i_Attack;
            return P663_i_Attack;
        }

        int F313_xxxx_CHAMPION_GetWoundDefense(ILiveEntity P652_i_ChampionIndex, WoundInfo P653_ui_WoundIndex)
        {
            int A0942_i_SlotIndex;
            int L0943_ui_ArmourShieldDefense = 0;
            bool L0944_B_UseSharpDefense;

            var L0946_ps_Champion = P652_i_ChampionIndex;//&G407_s_Party.Champions[P652_i_ChampionIndex];
            L0944_B_UseSharpDefense = P653_ui_WoundIndex.UseSharpDefense;
            //if (L0944_B_UseSharpDefense = M07_GET(P653_ui_WoundIndex, MASK0x8000_USE_SHARP_DEFENSE))
            //{
            //    M09_CLEAR(P653_ui_WoundIndex, MASK0x8000_USE_SHARP_DEFENSE);
            //}

            foreach (var hand in L0946_ps_Champion.Body.BodyParts.Where(b => b.Type == ActionHandStorageType.Instance || b.Type == ReadyHandStorageType.Instance))
            {
                var armor = hand.Storage[0] as Cloth;
                if (armor != null && armor.ClothItemFactoryBase.IsShield)
                {
                    L0943_ui_ArmourShieldDefense += ((F312_xzzz_CHAMPION_GetStrength(L0946_ps_Champion, hand.Type) +
                        F143_mzzz_DUNGEON_GetArmourDefense(armor, L0944_B_UseSharpDefense)) *
                        //G050_auc_Graphic562_WoundDefenseFactor[P653_ui_WoundIndex.WoundIndex]) 
                        hand.InjuryMultipler)
                        >> ((hand.Type == P653_ui_WoundIndex.WoundIndex) ? 4 : 5);
                }
            }


            int A0942_i_WoundDefense = rand.Next((L0946_ps_Champion.GetProperty(PropertyFactory<VitalityProperty>.Instance).Value >> 3) + 1);
            if (L0944_B_UseSharpDefense)
            {
                A0942_i_WoundDefense >>= 1;
            }

            //TODO add additional defens to  L0946_ps_Champion.ActionDefense which should be DefenseProperty
            A0942_i_WoundDefense += L0946_ps_Champion.GetProperty(PropertyFactory<DefenseProperty>.Instance).Value /*L0946_ps_Champion.ActionDefense*/ +
                L0946_ps_Champion.GetProperty(PropertyFactory<ShieldDefenseProperty>.Instance).Value +
                L0943_ui_ArmourShieldDefense;

            //TODO should be done by armor modifier property
            //if ((P653_ui_WoundIndex > C01_SLOT_ACTION_HAND) && 
            //    (M12_TYPE(L0945_T_Thing = L0946_ps_Champion->Slots[P653_ui_WoundIndex]) == C06_THING_TYPE_ARMOUR))
            //{
            //    L0947_ps_ArmourInfo = (ARMOUR_INFO*)F156_afzz_DUNGEON_GetThingData(L0945_T_Thing);
            //    A0942_i_WoundDefense += F143_mzzz_DUNGEON_GetArmourDefense(&G239_as_Graphic559_ArmourInfo[((ARMOUR*)L0947_ps_ArmourInfo)->Type], L0944_B_UseSharpDefense);
            //}

            //if (M07_GET(L0946_ps_Champion->Wounds, 1 << P653_ui_WoundIndex))
            if (L0946_ps_Champion.Body.GetBodyStorage(P653_ui_WoundIndex.WoundIndex).IsWound)
            {
                A0942_i_WoundDefense -= 8 + rand.Next(4);
            }

            //TODO party sleeping
            //if (G300_B_PartyIsSleeping)
            //{
            //    A0942_i_WoundDefense >>= 1;
            //}
            return MathHelper.Clamp(A0942_i_WoundDefense >> 1, 0, 100);
        }

        int F030_aaaW_MAIN_GetScaledProduct(int P036_ui_Value1, int P037_ui_Scale, int P038_ui_Value2)
        {
            return (P036_ui_Value1 * P038_ui_Value2) >> P037_ui_Scale;
        }

        int F143_mzzz_DUNGEON_GetArmourDefense(Cloth P239_ps_ArmourInfo, bool P240_B_UseSharpDefense)
        {
            int L0244_ui_Defense = P239_ps_ArmourInfo.ClothItemFactoryBase.ArmorStrength; //P239_ps_ArmourInfo->Defense;
            if (P240_B_UseSharpDefense)
            {
                L0244_ui_Defense = F030_aaaW_MAIN_GetScaledProduct(L0244_ui_Defense, 3, P239_ps_ArmourInfo.ClothItemFactoryBase.SharpResistance + 4);
            }
            return L0244_ui_Defense;
        }

        async void F322_lzzz_CHAMPION_Poison(ILiveEntity entity, int attack)
        {
            var health = entity.GetProperty(PropertyFactory<HealthProperty>.Instance);
            var poison = entity.GetProperty(PropertyFactory<PoisonProperty>.Instance);
            poison.Value++;

            var eventCount = attack;
            for (int i = 0; i < eventCount; i++, attack--)
            {
                //Todo add event on value changed to stop poison immediately 
                if (poison.Value == 0)
                    return;

                health.Value -= MathHelper.Max(1, attack >> 6);
                await Task.Delay(6000);
            }
            poison.Value--;
        }
    }
}