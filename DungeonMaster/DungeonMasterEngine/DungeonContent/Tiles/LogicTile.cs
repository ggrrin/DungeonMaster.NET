using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    //TODO look how the activation of "normal" actuators is done !!!!!!!


    public class LogicMessage : Message
    {
        public int Specifer { get; }

        public LogicMessage(MessageAction action, int specifer) : base(action)
        {
            Specifer = specifer;
        }
    }
    

    public sealed class LogicTile : Tile<LogicMessage>
    {
        public override void AcceptMessage(LogicMessage message)
        {
            //TODO logic with specifier 
        }

        public override bool IsAccessible => false;

        public IEnumerable<LogicGate> Gates { get; set; }

        public IEnumerable<CounterActuator> Counters { get; internal set; }

        public LogicTile(Vector3 position) : base(position)
        { }

        public override void ActivateTileContent() { }

        public override void DeactivateTileContent() { }

    }
    
}