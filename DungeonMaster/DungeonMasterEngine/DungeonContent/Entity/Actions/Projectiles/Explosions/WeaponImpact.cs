using DungeonMasterEngine.DungeonContent.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions.Projectiles.Explosions
{
    public class WeaponImpact : ThrowImpact<Weapon>
    {
        public WeaponImpact(Weapon item) : base(item) { }

        protected override AttackInfo GetDefaultImpact(Projectile projectile)
        {
            int attack = Item.FactoryType.KineticEnergy;
            attack += Item.FactoryBase.Weight >> 1;
            return new AttackInfo
            {
                Attack = attack,
                Type = CreatureAttackType.C3_ATTACK_BLUNT
            };
        }
    }
}