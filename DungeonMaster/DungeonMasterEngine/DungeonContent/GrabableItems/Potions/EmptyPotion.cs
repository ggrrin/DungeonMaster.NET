using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class EmptyPotion : Potion, IWaterJar
    {
        public EmptyPotion(PotionInitializer initializer):base(initializer)
        { }

        public IGrabableItem Fill(IFactories factories)
        {
            return factories.PotionFactories[15].Create(new PotionInitializer());
        }
    }
}