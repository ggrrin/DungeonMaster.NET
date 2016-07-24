using System;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actions
{
    public abstract class AttackBase<TFactory> : IAction<TFactory> where TFactory : IActionFactory
    {
        protected static readonly Random rand = new Random();


        protected AttackBase(TFactory factory)
        {
            Factory = factory;
        }

        IActionFactory IAction.Factory => Factory;

        public TFactory Factory { get; }

        public abstract int Apply(MapDirection direction);



        public static bool F308_vzzz_CHAMPION_IsLucky(ILiveEntity entity, int P646_ui_Percentage)
        {
            if (rand.Next(2) != 0 && (rand.Next(100) > P646_ui_Percentage))
                return true;

            var luckProperty = entity.GetProperty(PropertyFactory<LuckProperty>.Instance);
            bool AP646_ui_IsLucky = rand.Next(luckProperty.MaxValue) > P646_ui_Percentage;
            luckProperty.BaseValue += AP646_ui_IsLucky ? -2 : 2;
            return AP646_ui_IsLucky;
        }

        public static int F312_xzzz_CHAMPION_GetStrength(ILiveEntity attackProvider, IStorageType P651_i_SlotIndex)
        {
            int L0935_i_Strength = rand.Next(16) + attackProvider.GetProperty(PropertyFactory<StrengthProperty>.Instance).MaxValue;
            var L0938_T_Thing = attackProvider.Body.GetStorage(P651_i_SlotIndex).Storage.First();
            //if ((A0936_ui_ObjectWeight = F140_yzzz_DUNGEON_GetObjectWeight(L0938_T_Thing)) <= (A0937_ui_OneSixteenthMaximumLoad = F309_awzz_CHAMPION_GetMaximumLoad(L0939_ps_Champion) >> 4))
            int A0936_ui_ObjectWeight = (int)L0938_T_Thing.FactoryBase.Weight;
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
            if (attackProvider.Body.GetBodyStorage(P651_i_SlotIndex).IsWounded)
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
            //if ((L0925_i_CurrentStamina = P640_ps_Champion->CurrentStamina) < (L0926_i_HalfMaximumStamina = P640_ps_Champion->MaximumStamina >> 1))
            int L0925_i_CurrentStamina = staminaProperty.Value;
            int L0926_i_HalfMaximumStamina = staminaProperty.MaxValue >> 1;
            if (L0925_i_CurrentStamina < L0926_i_HalfMaximumStamina)
            {
                return (P641_i_Value >>= 1) + (int)(((long)P641_i_Value * (long)L0925_i_CurrentStamina) / L0926_i_HalfMaximumStamina);
            }
            return P641_i_Value;
        }


    }
}