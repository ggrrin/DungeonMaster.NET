namespace DungeonMasterEngine.DungeonContent.Tiles
{
    class MocapBaseInitializer : InitializerBase
    {
        public event Initializer<MocapBaseInitializer> Initializing;

        public float baseVAlue { get; set; }

        protected override void OnInitialing()
        {
            Initializing?.Invoke(this);
        }

        protected override void OnInitialized()
        {
            throw new System.NotImplementedException();
        }
    }
}