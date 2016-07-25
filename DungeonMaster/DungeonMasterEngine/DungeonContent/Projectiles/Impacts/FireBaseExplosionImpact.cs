using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.CreatureSpecific;

namespace DungeonMasterEngine.DungeonContent.Projectiles.Impacts
{
    public abstract class FireBaseExplosionImpact : ExplosionImpact
    {
        public override bool IsMagic => true; 

        protected override int ModifyAttackyByResistance(int attack, ILiveEntity entity)
        {
            var antiFire = entity.GetProperty(PropertyFactory<AntiFireProperty>.Instance);
            if (antiFire.Value == 15)
                return 0;

            if (entity.GetProperty(PropertyFactory<NonMaterialProperty>.Instance).Value == 1)
                attack >>= 2;

            attack -= rand.Next((antiFire.Value << 1) + 1);
            return attack;
        }

        public abstract override void FinishImpact(Projectile projectile);
    }
}