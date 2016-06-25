using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public sealed class LogicTile : Tile<Message>
    {
        public LogicActuator Actuator { get; private set; }

        public override void AcceptMessage(Message message)
        {
            Actuator.AcceptMessage(message);
        }

        public override bool IsAccessible => false;

        public LogicTile(LogicTileInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(LogicTileInitializer initializer)
        {
            Actuator = initializer.LogicActuator;

            initializer.Initializing -= Initialize;
        }

        public override IEnumerable<ITileSide> Sides => Enumerable.Empty<TileSide>();

        public override void ActivateTileContent() { }

        public override void DeactivateTileContent() { }

        public override IEnumerable<object> SubItems => Enumerable.Empty<object>();
    }
}