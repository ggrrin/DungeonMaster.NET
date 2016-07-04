using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class EePotion : DrinkablePotion 
    {

        public override bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;

            int A1085_ui_AdjustedPotionPower = (Power / 25) + 8;
            var manaProperty = entity.GetProperty(PropertyFactory<ManaProperty>.Instance);
            int A1088_ui_Mana = MathHelper.Min(900, (manaProperty.Value + A1085_ui_AdjustedPotionPower) + (A1085_ui_AdjustedPotionPower - 8));
            //bounds check should do property itself
            //if (A1088_ui_Mana > L1083_ps_Champion->MaximumMana)
            //{
            //    A1088_ui_Mana -= (A1088_ui_Mana - MathHelper.Max(L1083_ps_Champion->CurrentMana, L1083_ps_Champion->MaximumMana)) >> 1;
            //}
            manaProperty.Value = A1088_ui_Mana;
            return Used = true;
        }
    }
}