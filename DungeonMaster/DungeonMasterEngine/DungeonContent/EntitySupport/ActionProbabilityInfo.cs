namespace DungeonMasterEngine.DungeonContent.EntitySupport
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