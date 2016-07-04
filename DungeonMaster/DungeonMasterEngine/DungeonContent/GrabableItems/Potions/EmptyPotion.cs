using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Actuators;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class EmptyPotion : Potion, IWaterJar
    {
        public EmptyPotion(PotionInitializer initializer):base(initializer)
        { }

        public IGrabableItem Fill()
        {
            var res = new WaterPotion();
            res.Initialize(new PotionInitializer {Factory = base.Type.EmptyPotionCreator.WaterCreator});
            return res;
        }
    }
}