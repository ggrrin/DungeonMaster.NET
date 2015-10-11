using Microsoft.Xna.Framework;
using System;

namespace DungeonMasterEngine
{
    public class VirtualTile : Tile
    {
        public VirtualTile() :base(Vector3.Zero)
        {
        }

        public override bool IsAccessible => false;
    }
}