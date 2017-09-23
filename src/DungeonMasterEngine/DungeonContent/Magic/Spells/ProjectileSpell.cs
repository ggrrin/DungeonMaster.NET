using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Projectiles;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public abstract class ProjectileSpell : Spell
    {
        public abstract Projectile Projectile { get; protected set; }

        public override void Run(ILiveEntity caster, MapDirection direction)
        {
            Projectile.Run(caster.Location, direction);
        }
    }
}