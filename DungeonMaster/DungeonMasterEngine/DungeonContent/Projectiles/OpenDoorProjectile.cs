namespace DungeonMasterEngine.DungeonContent.Projectiles
{
    public class OpenDoorProjectile : Projectile
    {
        public OpenDoorProjectile(int kineticEnergy, int stepEnergy) : base(kineticEnergy, stepEnergy, 0)
        {
            
        }

        protected override void FinishImpact()
        {
            throw new System.NotImplementedException();
        }
    }
}