using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class MaPotion : DrinkablePotion
    {

        public override bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;
            int L1086_ui_Counter = ((511 - Power) / (32 + (Power + 1) / 8)) >> 1;
            var stamina = entity.GetProperty(PropertyFactory<StaminaProperty>.Instance);
            stamina.Value += stamina.MaxValue / L1086_ui_Counter;
            return Used = true;
        }
    }
}