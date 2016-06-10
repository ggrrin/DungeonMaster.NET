namespace DungeonMasterEngine.DungeonContent.Tiles
{
    class MocapBase
    {
        private float x;

        public MocapBase(MocapBaseInitializer initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(MocapBaseInitializer initializer)
        {
            x = initializer.baseVAlue;
            initializer.Initializing -= Initialize;
        }
    }
}