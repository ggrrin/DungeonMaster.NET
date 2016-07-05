using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent
{
    class Timm
    {

        //void F331_auzz_CHAMPION_ApplyTimeEffects_COPYPROTECTIONF()
        //{
        //    int A1006_ui_ChampionIndex;
        //    Champion L1010_ps_Champion;
        //    SCENT L1014_s_Scent;


        //    if (!G305_ui_PartyChampionCount)
        //    {
        //        return;
        //    }

        //    L1014_s_Scent.Location.MapX = G306_i_PartyMapX;
        //    L1014_s_Scent.Location.MapY = G307_i_PartyMapY;
        //    L1014_s_Scent.Location.MapIndex = G309_i_PartyMapIndex;
        //    int A1007_ui_ScentIndex = 0;
        //    while ((int)A1007_ui_ScentIndex < G407_s_Party.ScentCount - 1)
        //    {
        //        G407_s_Party.Scents[A1007_ui_ScentIndex] == L1014_s_Scent; /* BUG0_00 Useless code. The result of the comparison is ignored */
        //        if (!(G407_s_Party.ScentStrengths[A1007_ui_ScentIndex] = F025_aatz_MAIN_GetMaximumValue(0, G407_s_Party.ScentStrengths[A1007_ui_ScentIndex] - 1)) && !A1007_ui_ScentIndex)
        //        {
        //            F316_aizz_CHAMPION_DeleteScent(0);
        //            continue;
        //        }
        //        A1007_ui_ScentIndex++;
        //    }
        //    int A1006_ui_GameTime = G313_ul_GameTime;
        //    int L1012_ui_TimeCriteria = (((A1006_ui_GameTime & 0x0080) + ((A1006_ui_GameTime & 0x0100) >> 2)) + ((A1006_ui_GameTime & 0x0040) << 2)) >> 2;
        //    for (A1006_ui_ChampionIndex = C00_CHAMPION_FIRST, L1010_ps_Champion = G407_s_Party.Champions; A1006_ui_ChampionIndex < G305_ui_PartyChampionCount; A1006_ui_ChampionIndex++, L1010_ps_Champion++)
        //    {
        //        if (L1010_ps_Champion->CurrentHealth && (M00_INDEX_TO_ORDINAL(A1006_ui_ChampionIndex) != G299_ui_CandidateChampionOrdinal))
        //        {
        //            int A1008_ui_WizardSkillLevel;
        //            if ((L1010_ps_Champion->CurrentMana < L1010_ps_Champion->MaximumMana) && (L1012_ui_TimeCriteria < (L1010_ps_Champion->Statistics[C3_STATISTIC_WISDOM][C1_CURRENT] + (A1008_ui_WizardSkillLevel = F303_AA09_CHAMPION_GetSkillLevel(A1006_ui_ChampionIndex, C03_SKILL_WIZARD) + F303_AA09_CHAMPION_GetSkillLevel(A1006_ui_ChampionIndex, C02_SKILL_PRIEST)))))
        //            {
        //                int A1007_ui_ManaGain = L1010_ps_Champion->MaximumMana / 40;
        //                if (G300_B_PartyIsSleeping)
        //                {
        //                    A1007_ui_ManaGain = A1007_ui_ManaGain << 1;
        //                }
        //                A1007_ui_ManaGain++;
        //                F325_bzzz_CHAMPION_DecrementStamina(A1006_ui_ChampionIndex, A1007_ui_ManaGain * F025_aatz_MAIN_GetMaximumValue(7, 16 - A1008_ui_WizardSkillLevel));
        //                L1010_ps_Champion->CurrentMana += F024_aatz_MAIN_GetMinimumValue(A1007_ui_ManaGain, L1010_ps_Champion->MaximumMana - L1010_ps_Champion->CurrentMana);
        //            }
        //            else
        //            {
        //                if (L1010_ps_Champion->CurrentMana > L1010_ps_Champion->MaximumMana)
        //                {
        //                    L1010_ps_Champion->CurrentMana--;
        //                }
        //            }
        //            int A1009_i_SkillIndex;
        //            for (A1009_i_SkillIndex = C19_SKILL_WATER; A1009_i_SkillIndex >= C00_SKILL_FIGHTER; A1009_i_SkillIndex--)
        //            {
        //                if (L1010_ps_Champion->Skills[A1009_i_SkillIndex].TemporaryExperience > 0)
        //                {
        //                    L1010_ps_Champion->Skills[A1009_i_SkillIndex].TemporaryExperience--;
        //                }
        //            }
        //            int A1007_ui_StaminaGainCycleCount = 4;
        //            int A1009_i_StaminaMagnitude = L1010_ps_Champion->MaximumStamina;
        //            while (L1010_ps_Champion->CurrentStamina < (A1009_i_StaminaMagnitude >>= 1))
        //            {
        //                A1007_ui_StaminaGainCycleCount += 2;
        //            }
        //            int A1009_i_StaminaLoss = 0;
        //            int A1013_i_StaminaAmount = F026_a003_MAIN_GetBoundedValue(1, (L1010_ps_Champion->MaximumStamina >> 8) - 1, 6);
        //            if (G300_B_PartyIsSleeping)
        //            {
        //                    A1013_i_StaminaAmount <<= 1;
        //                }
        //            int A1008_ui_Delay;
        //            if ((A1008_ui_Delay = (G313_ul_GameTime - G362_l_LastPartyMovementTime)) > 80)
        //                {
        //                    A1013_i_StaminaAmount++;
        //                    if (A1008_ui_Delay > 250)
        //                    {
        //                        A1013_i_StaminaAmount++;
        //                    }
        //                }
        //                do
        //                {
        //                    int A1008_ui_StaminaAboveHalf = (A1007_ui_StaminaGainCycleCount <= 4);
        //                    if (L1010_ps_Champion->Food < -512)
        //                    {
        //                        if (A1008_ui_StaminaAboveHalf)
        //                        {
        //                            A1009_i_StaminaLoss += A1013_i_StaminaAmount;
        //                            L1010_ps_Champion->Food -= 2;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (L1010_ps_Champion->Food >= 0)
        //                        {
        //                            A1009_i_StaminaLoss -= A1013_i_StaminaAmount;
        //                        }
        //                        L1010_ps_Champion->Food -= A1008_ui_StaminaAboveHalf ? 2 : A1007_ui_StaminaGainCycleCount >> 1;
        //                    }
        //                    if (L1010_ps_Champion->Water < -512)
        //                    {
        //                        if (A1008_ui_StaminaAboveHalf)
        //                        {
        //                            A1009_i_StaminaLoss += A1013_i_StaminaAmount;
        //                            L1010_ps_Champion->Water -= 1;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (L1010_ps_Champion->Water >= 0)
        //                        {
        //                            A1009_i_StaminaLoss -= A1013_i_StaminaAmount;
        //                        }
        //                        L1010_ps_Champion->Water -= A1008_ui_StaminaAboveHalf ? 1 : A1007_ui_StaminaGainCycleCount >> 2;
        //                    }
        //                    A1007_ui_StaminaGainCycleCount--;
        //                } while (A1007_ui_StaminaGainCycleCount && ((L1010_ps_Champion->CurrentStamina - A1009_i_StaminaLoss) < L1010_ps_Champion->MaximumStamina));
        //                F325_bzzz_CHAMPION_DecrementStamina(A1006_ui_ChampionIndex, A1009_i_StaminaLoss);
        //                if (L1010_ps_Champion->Food < -1024)
        //                {
        //                    L1010_ps_Champion->Food = -1024;
        //                }
        //                if (L1010_ps_Champion->Water < -1024)
        //                {
        //                    L1010_ps_Champion->Water = -1024;
        //                }
        //                if ((L1010_ps_Champion->CurrentHealth < L1010_ps_Champion->MaximumHealth) && (L1010_ps_Champion->CurrentStamina >= (L1010_ps_Champion->MaximumStamina >> 2)) && (L1012_ui_TimeCriteria < (L1010_ps_Champion->Statistics[C4_STATISTIC_VITALITY][C1_CURRENT] + 12)))
        //                {
        //                    int A1013_i_HealthGain = (L1010_ps_Champion->MaximumHealth >> 7) + 1;
        //                    if (G300_B_PartyIsSleeping)
        //                    {
        //                    A1013_i_HealthGain <<= 1;
        //                }
        //                if (F033_aaaz_OBJECT_GetIconIndex(L1010_ps_Champion->Slots[C10_SLOT_NECK]) == C121_ICON_JUNK_EKKHARD_CROSS)
        //                {
        //                    A1013_i_HealthGain += (A1013_i_HealthGain >> 1) + 1;
        //                }
        //                L1010_ps_Champion->CurrentHealth += F024_aatz_MAIN_GetMinimumValue(A1013_i_HealthGain, L1010_ps_Champion->MaximumHealth - L1010_ps_Champion->CurrentHealth);
        //            }
        //            if (G328_i_TimeBombToKillParty_COPYPROTECTIONF && !(--G328_i_TimeBombToKillParty_COPYPROTECTIONF))
        //            {
        //                G524_B_RestartGameAllowed = FALSE;
        //                F324_aezz_CHAMPION_DamageAll_GetDamagedChampionCount(4096, MASK0x0000_NO_WOUND, C0_ATTACK_NORMAL);
        //            }
        //                if (!((int)G313_ul_GameTime & (G300_B_PartyIsSleeping ? 63 : 255)))
        //                {
        //                    int A1007_ui_StatisticIndex = C0_STATISTIC_LUCK;
        //                    while (A1007_ui_StatisticIndex <= C6_STATISTIC_ANTIFIRE)
        //                    {
        //                        char L1011_puc_Statistic = L1010_ps_Champion->Statistics[A1007_ui_StatisticIndex];

        //                            int A1008_ui_StatisticMaximum = L1011_puc_Statistic[C0_MAXIMUM];
        //                            if (L1011_puc_Statistic[C1_CURRENT] < A1008_ui_StatisticMaximum)
        //                            {
        //                                L1011_puc_Statistic[C1_CURRENT]++;
        //                            }
        //                            else
        //                            {
        //                                if (L1011_puc_Statistic[C1_CURRENT] > A1008_ui_StatisticMaximum)
        //                                {
        //                                    L1011_puc_Statistic[C1_CURRENT] -= L1011_puc_Statistic[C1_CURRENT] / A1008_ui_StatisticMaximum;
        //                                }
        //                            }
        //                            A1007_ui_StatisticIndex++;
        //                        }
        //                }
        //            if (!G300_B_PartyIsSleeping && (L1010_ps_Champion->Direction != G308_i_PartyDirection) && (G361_l_LastCreatureAttackTime < (G313_ul_GameTime - 60)))
        //                    {
        //                        L1010_ps_Champion->Direction = G308_i_PartyDirection;
        //                        L1010_ps_Champion->MaximumDamageReceived = 0;
        //                        M08_SET(L1010_ps_Champion->Attributes, MASK0x0400_ICON);
        //                    }
        //                    M08_SET(L1010_ps_Champion->Attributes, MASK0x0100_STATISTICS);
        //                    if (M00_INDEX_TO_ORDINAL(A1006_ui_ChampionIndex) == G423_i_InventoryChampionOrdinal)
        //                    {
        //                        if (G333_B_PressingMouth || G331_B_PressingEye || (G424_i_PanelContent == C0_PANEL_FOOD_WATER_POISONED))
        //                        {
        //                            M08_SET(L1010_ps_Champion->Attributes, MASK0x0800_PANEL);
        //                        }
        //                    }
        //                }
        //            }
        //            F293_ahzz_CHAMPION_DrawAllChampionStates();
        //        }
            }
}