using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Projectiles.Impacts
{
    public abstract class ExplosionImpact : Impact
    {
        public override bool IsMagic => true; 

        protected abstract override AttackInfo GetDefaultImpact(Projectile projectile);

        protected virtual int ModifyAttackyByResistance(int attack, ILiveEntity entity)
        {
            return attack;
        }

        protected virtual void F191_aayz_GROUP_GetDamageAllCreaturesOutcome(int attack, Entity.GroupSupport.Base.ISpaceRouteElement location)
        {

            foreach (var liveEntity in location.Tile.LayoutManager.Entities.ToArray())
            {
                attack = ModifyAttackyByResistance(attack, liveEntity );
                if (attack > 0)
                {
                    int L0385_i_RandomAttack;
                    attack -= (L0385_i_RandomAttack = (attack >> 3) + 1);
                    L0385_i_RandomAttack <<= 1;


                    liveEntity.GetProperty(PropertyFactory<HealthProperty>.Instance).Value -= attack + rand.Next(L0385_i_RandomAttack);
                }
            }

        }
    }
}