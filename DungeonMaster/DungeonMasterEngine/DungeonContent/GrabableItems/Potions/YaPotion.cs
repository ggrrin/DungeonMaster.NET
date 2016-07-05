using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.ChampionSpecific;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class YaPotion : DrinkablePotion
    {

        public async void DisableEffectAsync(IProperty property, int miliseconds, int correction)
        {
            await Task.Delay(miliseconds);
            property.Value -= correction;
        }

        public override bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;
            int A1085_ui_AdjustedPotionPower = (Power / 25) + 8;
            A1085_ui_AdjustedPotionPower += A1085_ui_AdjustedPotionPower >> 1;

            var shieldDefense = entity.GetProperty(PropertyFactory<ShieldDefenseProperty>.Instance);

            if (shieldDefense.Value > 50)
            {
                A1085_ui_AdjustedPotionPower >>= 2;
            }

            shieldDefense.Value += A1085_ui_AdjustedPotionPower;
            DisableEffectAsync(shieldDefense, (A1085_ui_AdjustedPotionPower * A1085_ui_AdjustedPotionPower) * 1000 / 6, -A1085_ui_AdjustedPotionPower);

            return Used = true;
        }
    }
}