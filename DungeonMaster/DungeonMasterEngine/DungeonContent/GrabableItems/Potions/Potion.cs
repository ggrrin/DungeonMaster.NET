using System;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class Potion : GrabableItem
    {
        protected static Random rand = new Random();
        public PotionFactory Type { get; private set; }
        public override IGrabableItemFactoryBase FactoryBase => Type;
        public int Power { get; private set; }

        protected Potion() {}

        private bool initialized = false;
        public Potion(IPotionInitializer initializer)
        {
            InitializePotion(initializer);
        }

        protected void InitializePotion(IPotionInitializer initializer)
        {
            if(initialized)
                throw new InvalidOperationException();

            Power = initializer.PotionPower;
            Type = initializer.Factory;
            initialized = true;
        }
    }
}