using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders
{
    public class WeaponInitializator : IWeaponInitializer {
        public bool IsBroken { get; set; }
        public int ChargeCount { get; set; }
        public bool IsPoisoned { get; set; }
        public bool IsCursed { get; set; }
    }
}