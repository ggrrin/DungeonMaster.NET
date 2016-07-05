using DungeonMasterEngine.DungeonContent.Actions;
using DungeonMasterEngine.DungeonContent.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Projectiles.Impacts
{
    public class ThrowImpact<TItem> : Impact where TItem : IGrabableItem
    {
        public TItem Item { get; }

        public ThrowImpact(TItem item)
        {
            Item = item;
        }

        public override bool CreatesExplosion => false; 

        protected override AttackInfo GetDefaultImpact(Projectile projectile)
        {
            int attack = rand.Next(4);
            attack += Item.FactoryBase.Weight >> 1;
            return new AttackInfo
            {
                Type = CreatureAttackType.C3_ATTACK_BLUNT,
                Attack = attack
            };
        }

        public override void FinishImpact(Projectile projectile)
        {
            Item.Location = projectile.Location;
        }
    }
}