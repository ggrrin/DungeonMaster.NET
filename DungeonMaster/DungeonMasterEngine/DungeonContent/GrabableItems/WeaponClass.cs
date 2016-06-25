namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public enum WeaponClass
    {
        /* Weapon info class */
        C000_CLASS_SWING_WEAPON = 0, /* Class 0: SWING weapons */
        C002_CLASS_DAGGER_AND_AXES = 2, /* Class 1 to 15: THROW weapons */
        C010_CLASS_BOW_AMMUNITION = 10,
        C011_CLASS_SLING_AMMUNITION = 11,
        C012_CLASS_POISON_DART = 12,
        C016_CLASS_FIRST_BOW = 16, /* Class 16 to 111: SHOOT weapons */
        C031_CLASS_LAST_BOW = 31,
        C032_CLASS_FIRST_SLING = 32,
        C047_CLASS_LAST_SLING = 47,
        C112_CLASS_FIRST_MAGIC_WEAPON = 112 /* Class 112 to 255: Magic and special weapons */

        /* Weapon attributes */
        //#define M65_SHOOT_ATTACK(attributes)              ((attributes) & 0x00FF)
        //#define M66_PROJECTILE_ASPECT_ORDINAL(attributes) (((attributes) >> 8) & 0x001F)
    }
}