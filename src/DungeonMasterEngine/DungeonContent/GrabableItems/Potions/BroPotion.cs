using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class BroPotion: DrinkablePotion
    {
        public override bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;

            entity.GetProperty(PropertyFactory<PoisonProperty>.Instance).Value = 0;
            return Used = true;
        }
    }
}