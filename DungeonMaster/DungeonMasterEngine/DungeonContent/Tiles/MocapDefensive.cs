namespace DungeonMasterEngine.DungeonContent.Tiles
{
    class MocapDefensive : MocapBase
    {
        private readonly MocapDefensiveInitializer initializer;
        private string avalue;

        public MocapDefensive(MocapDefensiveInitializer initializer) : base(initializer)
        {
            this.initializer = initializer;
            initializer.Initializing += Initialize;
        }

        private void Initialize(MocapDefensiveInitializer initalizer)
        {
            avalue = initalizer.AValue;

            initalizer.Initializing -= Initialize;
        }

    }
}