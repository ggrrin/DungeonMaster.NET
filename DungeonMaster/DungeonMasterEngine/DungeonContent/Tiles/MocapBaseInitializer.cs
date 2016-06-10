namespace DungeonMasterEngine.DungeonContent.Tiles
{
    class MocapBaseInitializer : InitializerBase
    {
        public event Initializer<MocapBaseInitializer> Initializing;

        public float baseVAlue { get; set; }

        protected override void OnInitialize()
        {
            Initializing?.Invoke(this);
        }
    }
}