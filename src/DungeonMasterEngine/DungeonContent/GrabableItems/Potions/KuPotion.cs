using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class KuPotion : DrinkablePotion
    {

        public override bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;
            F348_xxxx_INVENTORY_AdjustStatisticCurrentValue(entity, PropertyFactory<StrengthProperty>.Instance, Power / 35 + 5); /* Value between 5 and 12 */
            return Used = true;
        }
    }
}