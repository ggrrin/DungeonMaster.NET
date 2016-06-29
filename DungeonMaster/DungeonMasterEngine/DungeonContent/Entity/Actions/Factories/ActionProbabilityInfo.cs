namespace DungeonMasterEngine.DungeonContent.Entity.Actions.Factories
{
    public struct ActionProbabilityInfo
    {
        public int Value;
        public bool HitNonmaterial;

        public ActionProbabilityInfo(int value) : this()
        {
            Value = value;
        }

    }
}