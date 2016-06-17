namespace DungeonMasterEngine.DungeonContent.Tiles
{
    class MocapDefensiveInitializer : MocapBaseInitializer
    {
        public string AValue { get; set; }
        public new event Initializer<MocapDefensiveInitializer> Initializing;

        protected override void OnInitialing()
        {
            base.OnInitialing();
            Initializing?.Invoke(this);
        }
    }
}