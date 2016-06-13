using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class TeleprotInitializer : FloorInitializer
    {
        public new event Initializer<TeleprotInitializer> Initializing;

        public int NextLevelIndex { get; set; }
        public Point TargetTilePosition { get; set; }
        public MapDirection Direction { get; set; }
        public bool AbsoluteDirection { get; set; }
        public bool Open { get; set; }
        public bool Visible { get; set; }
        public IConstrain ScopeConstrain { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Initializing?.Invoke(this);
        }
    }
}