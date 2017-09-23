using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public class TorchWeapon : Weapon, ILightSource
    {
        private double lightTime = 0;
        public int LightPower { get; protected set; }

        public TorchWeapon(IWeaponInitializer initializer, TorchWeaponItemFactory weaponItemFactory) : base(initializer, weaponItemFactory)
        {
            LightPower = initializer.ChargeCount;
        }

        public void Update(GameTime time)
        {
            if (LightPower > 0)
            {
                lightTime += time.ElapsedGameTime.TotalMilliseconds;
                if (lightTime >= 85000 )
                {
                    lightTime = 0;
                    LightPower--;
                }
            }
        }
    }
}