namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public enum CreatureAttackType
    {
        C0_ATTACK_NORMAL = 0, /* Caused when a champion is poisoned, lacks stamina or stands in a Poison Cloud and by creatures (Giggler). This attack type cannot cause wounds */
        C1_ATTACK_FIRE = 1, /* Caused by Fireball explosions (or Lightning Bolt explosions if created right on the cell of a champion on the party square) and creatures (Black Flame) */
        C2_ATTACK_SELF = 2, /* Caused by the party walking into a wall, falling through a pit or standing in a closing door */
        C3_ATTACK_BLUNT = 3, /* Caused by all non explosion projectiles, Slime explosions and creatures (Demon, Mummy, Ruster, Stone Golem, Swamp Slime / Slime Devil, Trolin / Ant Man, Water Elemental) */
        C4_ATTACK_SHARP = 4, /* Caused by creatures (Animated Armour / Deth Knight, Couatl, Giant Scorpion / Scorpion, Giant Wasp / Muncher, Magenta Worm / Worm, Oitu, Pain Rat / Hellhound, Red Dragon / Dragon, Rockpile / Rock pile, Skeleton) */
        C5_ATTACK_MAGIC = 5, /* Caused by Poison Bolt explosions and creatures (Grey Lord, Lord Chaos, Lord Order, Materializer / Zytaz, Vexirk, Wizard Eye / Flying Eye) */
        C6_ATTACK_PSYCHIC = 6, /* Caused by creatures (Ghost / Rive, Screamer) */

        C7_ATTACK_LIGHTNING = 7, /* Caused by Lightning Bolt explosions */
    }
}