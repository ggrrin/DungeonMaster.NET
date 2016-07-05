using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Projectiles;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public class OpenDoorSpell : ProjectileSpell
    {
        public override Projectile Projectile { get; protected set; }

        public override void Run(ILiveEntity caster, MapDirection direction)
        {
        }

        public OpenDoorSpell(int kineticEnergy, int stepEnergy)
        {
            Projectile = new OpenDoorProjectile(kineticEnergy, stepEnergy);
        }
    }
}