using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Projectiles
{
    public class OpenDoorProjectile : Projectile
    {
        public OpenDoorProjectile(int kineticEnergy, int stepEnergy) : base(kineticEnergy, stepEnergy, 0)
        {

        }

        protected override bool TryApplyBeforeMoving(ISpaceRouteElement newLocation)
        {
            var doorTile = Location.Tile as DoorTile;
            if (doorTile != null)
            {
                if (doorTile.HasButton && !doorTile.Door.Open)
                {
                    doorTile.ActivateTileContent();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void FinishImpact()
        {
        }
    }
}