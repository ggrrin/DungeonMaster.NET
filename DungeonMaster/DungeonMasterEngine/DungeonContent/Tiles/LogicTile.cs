using Microsoft.Xna.Framework;
using System;
using DungeonMasterEngine.DungeonContent.Items.Actuators;
using System.Linq;
using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class LogicTile : Tile
    {
        public override bool IsAccessible => false;

        public IEnumerable<LogicGate> Gates { get; set; }
        public IEnumerable<Counter> Counters { get; internal set; }

        public LogicTile(Vector3 position) : base(position)
        { }

        public override void ActivateTileContent() { }

        public override void DeactivateTileContent() { }

        public override void ExecuteContentActivator(ITileContentActivator contentActivator)
        {
            contentActivator.ActivateContent(this);
        }
    }
    
}