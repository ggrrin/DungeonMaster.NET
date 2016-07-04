using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class WaterPotion : DrinkablePotion
    {

        public override bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;
            var water = entity.GetProperty(PropertyFactory<WaterProperty>.Instance);
            water.Value += 1600; 
            return Used = true;
        }
    }
}