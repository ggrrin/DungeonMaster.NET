namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class WallIlusionInitializer : FloorInitializer 
    {
        public new event Initializer<WallIlusionInitializer> Initializing;

        public bool Imaginary { get; set; }
        public bool Open { get; set; }
        public bool RandomDecoration { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Initializing?.Invoke(this);

        }
    }
}