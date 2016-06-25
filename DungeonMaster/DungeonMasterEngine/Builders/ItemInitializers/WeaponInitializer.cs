using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders.ItemInitializers
{
    public class WeaponInitializer : IWeaponInitializer {
        public bool IsBroken { get; set; }
        public int ChargeCount { get; set; }
        public bool IsPoisoned { get; set; }
        public bool IsCursed { get; set; }
    }
}