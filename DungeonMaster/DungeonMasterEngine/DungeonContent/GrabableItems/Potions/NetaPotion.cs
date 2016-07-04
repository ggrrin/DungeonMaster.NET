using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class NetaPotion : DrinkablePotion
    {

        public override bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;
            int A1085_ui_AdjustedPotionPower = (Power / 25) + 8;
            F348_xxxx_INVENTORY_AdjustStatisticCurrentValue(entity, PropertyFactory<VitalityProperty>.Instance, A1085_ui_AdjustedPotionPower);
            return Used = true;
        }
    }
}