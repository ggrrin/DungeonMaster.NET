using System;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterParser.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class TeleportTile : Teleport<Message>
    {
        public TeleportTile(TeleprotInitializer initializer) : base(initializer) { }
    }
}
