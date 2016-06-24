namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public class WallIlusionInitializer : FloorInitializer 
    {
        public new event Initializer<WallIlusionInitializer> Initializing;

        public bool Imaginary { get; set; }
        public bool Open { get; set; }
        public bool RandomDecoration { get; set; }

        protected override void OnInitialing()
        {
            base.OnInitialing();
            Initializing?.Invoke(this);

        }
    }
}