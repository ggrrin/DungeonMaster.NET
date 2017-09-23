using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class RosPotion : DrinkablePotion
    {

        public override bool ApplyEffect(ILiveEntity entity)
        {
            int A1085_ui_AdjustedPotionPower = (Power / 25) + 8;
            F348_xxxx_INVENTORY_AdjustStatisticCurrentValue(entity, PropertyFactory<DextrityProperty>.Instance, A1085_ui_AdjustedPotionPower);
            return Used = true;
        }
    }
}