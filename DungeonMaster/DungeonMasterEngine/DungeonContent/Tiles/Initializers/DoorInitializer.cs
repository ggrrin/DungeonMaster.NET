namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public class DoorInitializer : FloorInitializer
    {
        public new event Initializer<DoorInitializer> Initializing;

        public MapDirection Direction { get; set;  }
        public Door Door { get; set; }
        public bool HasButton { get; set; }

        protected override void OnInitialing()
        {
            base.OnInitialing();
            Initializing?.Invoke(this);
        }
    }
}