using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Skills
{
    interface ISkillFactory
    {
        long Experience { get; set; }
        long TemporaryExperience { get; set; }

        void ModifySkill(IEntity attackProvider, ref int a0915UiStaminaAmount, ref int ap638UiSkillLevelAfter, int l0921IMajorStatisticIncrease, int l0920IMinorStatisticIncrease);
        //TODO define in skillsw
        //                        switch (L0916_i_BaseSkillIndex)
        //                {
        //                    case C00_SKILL_FIGHTER:
        //                        A0915_ui_StaminaAmount >>= 4;
        //                        AP638_ui_SkillLevelAfter *= 3;
        //                        L0919_ps_Champion->Statistics[C1_STATISTIC_STRENGTH][C0_MAXIMUM] += L0921_i_MajorStatisticIncrease;
        //                        L0919_ps_Champion->Statistics[C2_STATISTIC_DEXTERITY][C0_MAXIMUM] += L0920_i_MinorStatisticIncrease;
        //                        break;
        //                    case C01_SKILL_NINJA:
        //                        A0915_ui_StaminaAmount /= 21;
        //                        AP638_ui_SkillLevelAfter <<= 1;
        //                        L0919_ps_Champion->Statistics[C1_STATISTIC_STRENGTH][C0_MAXIMUM] += L0920_i_MinorStatisticIncrease;
        //                        L0919_ps_Champion->Statistics[C2_STATISTIC_DEXTERITY][C0_MAXIMUM] += L0921_i_MajorStatisticIncrease;
        //                        break;
        //                    case C03_SKILL_WIZARD:
        //                        A0915_ui_StaminaAmount >>= 5;
        //                        L0919_ps_Champion->MaximumMana += AP638_ui_SkillLevelAfter + (AP638_ui_SkillLevelAfter >> 1);
        //                        L0919_ps_Champion->Statistics[C3_STATISTIC_WISDOM][C0_MAXIMUM] += L0921_i_MajorStatisticIncrease;
        //                        goto T304_016;
        //                    case C02_SKILL_PRIEST:
        //                        A0915_ui_StaminaAmount /= 25;
        //                        L0919_ps_Champion->MaximumMana += AP638_ui_SkillLevelAfter;
        //                        AP638_ui_SkillLevelAfter += (AP638_ui_SkillLevelAfter + 1) >> 1;
        //                        L0919_ps_Champion->Statistics[C3_STATISTIC_WISDOM][C0_MAXIMUM] += L0920_i_MinorStatisticIncrease;
        //                        T304_016:
        //                        if ((L0919_ps_Champion->MaximumMana += F024_aatz_MAIN_GetMinimumValue(M04_RANDOM(4), L0922_i_BaseSkillLevel - 1)) > 900)
        //                        {
        //                            L0919_ps_Champion->MaximumMana = 900;
        //                        }
        //L0919_ps_Champion->Statistics[C5_STATISTIC_ANTIMAGIC][C0_MAXIMUM] += M02_RANDOM(3);
        //                        break;
        //                }
    }
}