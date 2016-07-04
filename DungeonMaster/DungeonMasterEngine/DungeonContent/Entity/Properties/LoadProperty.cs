using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.Actions;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class LoadProperty : Property
    {
        private readonly ILiveEntity liveEntity;
        public override int MaxValue => F309_awzz_CHAMPION_GetMaximumLoad();

        public override int BaseValue
        {
            get { return MaxValue; }
            set { throw new System.NotImplementedException(); }
        }

        public override IPropertyFactory Type  => PropertyFactory<LoadProperty>.Instance;

        public LoadProperty(ILiveEntity liveEntity)
        {
            this.liveEntity = liveEntity;
        }

        int F309_awzz_CHAMPION_GetMaximumLoad( /*P647_ps_Champion*/)
        {
            int L0929_ui_MaximumLoad;
            int L0930_i_Wounds;

            L0929_ui_MaximumLoad = (liveEntity.GetProperty(PropertyFactory<StrengthProperty>.Instance).Value << 3) + 100;//  (P647_ps_Champion->Statistics[C1_STATISTIC_STRENGTH][C1_CURRENT] << 3) + 100;
            L0929_ui_MaximumLoad = MeleeAttack.F306_xxxx_CHAMPION_GetStaminaAdjustedValue(/*P647_ps_Champion,*/liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance), L0929_ui_MaximumLoad);
            //if (L0930_i_Wounds = P647_ps_Champion->Wounds)
            if (liveEntity.Body.BodyParts.Any(b => b.IsWound))
            {
                L0929_ui_MaximumLoad -= L0929_ui_MaximumLoad >> (liveEntity.Body.BodyParts.First(b => b.Type == LegsStorageType.Instance).IsWound ? 2 : 3);// (M07_GET(L0930_i_Wounds, MASK0x0010_LEGS) ? 2 : 3);
            }
            //if (F033_aaaz_OBJECT_GetIconIndex(P647_ps_Champion->Slots[C05_SLOT_FEET]) == C119_ICON_ARMOUR_ELVEN_BOOTS)
            //{
            //    L0929_ui_MaximumLoad += L0929_ui_MaximumLoad >> 4;
            //}
            L0929_ui_MaximumLoad += AdditionalValues.Sum(x => x.Value);
            //
            L0929_ui_MaximumLoad += 9;
            L0929_ui_MaximumLoad -= L0929_ui_MaximumLoad % 10; /* Round the value to the next multiple of 10 */
            return L0929_ui_MaximumLoad;
        }
    }
}