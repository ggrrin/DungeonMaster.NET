using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class DoorInitializer : FloorInitializer
    {
        public new event Initializer<DoorInitializer> Initializing;

        public MapDirection Direction { get; set;  }
        public Door Door { get; set; }
        public bool HasButton { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Initializing?.Invoke(this);
        }
    }
}