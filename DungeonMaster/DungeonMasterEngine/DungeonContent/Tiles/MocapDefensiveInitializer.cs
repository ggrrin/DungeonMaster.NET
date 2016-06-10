namespace DungeonMasterEngine.DungeonContent.Tiles
{
    class MocapDefensiveInitializer : MocapBaseInitializer
    {
        public string AValue { get; set; }
        public new event Initializer<MocapDefensiveInitializer> Initializing;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Initializing?.Invoke(this);
        }
    }
}