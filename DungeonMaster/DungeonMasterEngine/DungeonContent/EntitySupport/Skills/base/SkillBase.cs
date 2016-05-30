using System;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Skills.@base
{
    public abstract class SkillBase : ISkill 
    {
        protected static readonly Random rand = new Random();

        protected readonly IEntity entity;

        public abstract ISkillFactory Factory { get; }  

        public long Experience { get; private set; }
        public long TemporaryExperience { get; private set; }

        public abstract SkillBase BaseSkill { get; }

        public int SkillLevel => F303_AA09_CHAMPION_GetSkillLevel(true) + 0;//TODO + modifers items
        public int BaseSkillLevel => F303_AA09_CHAMPION_GetSkillLevel(false);

        protected SkillBase(IEntity entity)
        {
            this.entity = entity;
        }

        protected abstract void ApplySkills( int majorIncrease, int minorIncrease);

        public void AddExperience(int experience)
        {
            F304_apzz_CHAMPION_AddSkillExperience( experience);
        }

        void F304_apzz_CHAMPION_AddSkillExperience(int exp)
        {
            //TODO finish
            //if ((P637_ui_SkillIndex >= C04_SKILL_SWING) && (P637_ui_SkillIndex <= C11_SKILL_SHOOT) && (G361_l_LastCreatureAttackTime < (G313_ul_GameTime - 150)))
            //{
            //    P638_ui_Experience >>= 1;
            //}

            if (exp > 0)
            {
                var mapDifficulty = entity.Location.Tile.LevelIndex; //TODO make it right int[14] { 0, 1, 1, 2, 2, 2, 3, 3, 3, 4, 5, 5, 6, 6 }
                if (mapDifficulty > 0)
                    exp *= mapDifficulty;

                var skill = BaseSkill ?? this;

                int levelBefore = skill.BaseSkillLevel;

                //TODO finish
                //if (BaseSkill != null &&  (G361_l_LastCreatureAttackTime > (G313_ul_GameTime - 25)) )
                //    exp <<= 1;

                Experience += exp;
                if (TemporaryExperience < 32000)
                    TemporaryExperience += MathHelper.Clamp(exp >> 3, 1, 100);

                if (skill.BaseSkill != null)
                    skill.BaseSkill.Experience += exp;

                int levelAfter = skill.BaseSkillLevel;

                if (levelAfter > levelBefore)
                    skill.LevelUp();//always level up only base skills
            }
        }

        protected virtual void LevelUp()
        {
            int minorIncrease = rand.Next(2);
            int majorIncrease = 1 + rand.Next(2);
            entity.GetProperty(PropertyFactory<AntiFireProperty>.Instance).BaseValue += rand.Next(2) & ~BaseSkillLevel;
            ApplySkills(majorIncrease, minorIncrease);
        }

        int F303_AA09_CHAMPION_GetSkillLevel(bool applyTemporaryExperience)
        {
            //TODO party sleeping
            //if (G300_B_PartyIsSleeping)
            //{
            //    return 1;
            //}

            long exp = Experience;
            if (applyTemporaryExperience)
            {
                exp += TemporaryExperience;
                exp += BaseSkill?.TemporaryExperience ?? 0;
            }

            if (BaseSkill != null)
            {
                exp += BaseSkill.Experience;
                exp /= 2;
            }

            int skillLevel = 1;
            while (exp >= 500)
            {
                exp >>= 1;
                skillLevel++;
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
            return skillLevel;
        }
    }
}