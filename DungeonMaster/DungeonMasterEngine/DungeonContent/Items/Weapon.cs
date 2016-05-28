using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    internal class Weapon : GrabableItem
    {
        public Weapon(Vector3 position) : base(position)
        {
        }

        public int Strength { get; set; }
        public WeaponClass Class { get; set; }
    }
}