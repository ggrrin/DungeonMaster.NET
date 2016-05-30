namespace DungeonMasterEngine.DungeonContent.Entity
{
    internal struct ActionProbabilityInfo
    {
        public int Value;
        public bool HitNonmaterial;

        public ActionProbabilityInfo(int value) : this()
        {
            Value = value;
        }

    }
}