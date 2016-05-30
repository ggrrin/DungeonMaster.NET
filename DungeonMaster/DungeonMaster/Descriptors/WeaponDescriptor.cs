namespace DungeonMasterParser.Descriptors
{
    public class WeaponDescriptor : DescriptorBase
    {
        //http://dmweb.free.fr/?q=node/1395#toc46
        //unsigned char Weight;
        //1 byte: item weight(100 g units)

        //unsigned char Class;
        //1 byte: Weapon class, with delta value for range weapons(the value substracted from the energy of the missile at each of its movement). This value is used to determine if ammunition fits with a weapon.
        //The lowest delta values of range weapons should correspond to the best weapons but in the data it is the opposite.
        //00 Normal weapon
        //02 Axes.
        //01, 03-0F Range weapon. 0Ah is for Arrows (used with Bow) and 0Bh is for Rocks(used with Sling)
        //10-1F Bow(Remove 16 from this value to obtain the delta value).
        //20-2F Sling(Remove 32 from this value to obtain the delta value).
        //30-6F No such weapons
        //70-7F Weapons that can throw missiles(Fury is of type 00)
        //80-9F Magical item(like Staff or Ring)
        //C0-FF Not a weapon(like Horn of fear, Firestaff)
        public int Class { get; set; }
        public int?  DeltaEnergy { get; set; }

        //unsigned char Strength;
        //1 byte: Damage.Applies to both normal and range weapons.
        public int Strength { get; set; } //Applies to both normal and range weapons.

        //unsigned char KineticEnergy;
        //1 byte: Initial energy when the weapon is thrown(Distance).
        public int KineticEnergy { get; set; } 

        //unsigned int Attributes; /* Bits 15-13 Unreferenced */
        //1 word(big endian):
        //Bits 15-14: Unused, always 0
        //Bit 13: Unused.This bit is sometimes 0 and sometimes 1, but it is not used anywhere in the code and does not have any obvious meaning.
        //Bits 12-8: Missile image to display when the item is thrown.This index refers to the 'Missile graphics definitions' in item 558.
        //0: Same graphic as the one used to display the item on floor
        //1: Arrow
        //2: Dagger
        //3: Axe
        //4: Unused(it would be a lightning)
        //5: Slayer arrow
        //6: Stone club
        //7: Wooden club
        //8: Dart
        //9: Sword
        //A: Throwing star
        //Bits 7-0: Shoot damage bonus applied when the weapon is used with ammunition(signed value). It applies both to physical and magical missiles shot with the weapon.
        public int ShootDamage { get; set; }
    }
}