using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    //TODO look how the activation of "normal" actuators is done !!!!!!!


    public class LogicTileInitializer : TileInitializer
    {
        public new event Initializer<LogicTileInitializer> Initializing;
        public ActuatorX Actuator { get; }
    }

    public sealed class LogicTile : Tile<Message>
    {
        public override void AcceptMessage(Message message)
        {
            //TODO logic with specifier 
        }

        public override bool IsAccessible => false;

        public IEnumerable<LogicGate> Gates { get; set; }

        //public IEnumerable<CounterActuator> Counters { get; internal set; }

        public LogicTile(LogicTileInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;

        }

        private void Initialize(LogicTileInitializer initializer)
        {
            initializer.Initializing -= Initialize;
        }

        public override void ActivateTileContent() { }

        public override void DeactivateTileContent() { }

    }
    
}