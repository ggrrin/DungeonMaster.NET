namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public struct ProjectileProperties
    {
        public int KineticEnergy { get; }
        public int StepEnergy { get; }
        public int Attack { get; }

        public ProjectileProperties(int kineticEnergy, int stepEnergy, int attack)
        {
            KineticEnergy = kineticEnergy;
            StepEnergy = stepEnergy;
            Attack = attack;
        }

    }
}