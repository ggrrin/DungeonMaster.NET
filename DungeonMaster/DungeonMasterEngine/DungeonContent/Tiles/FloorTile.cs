using System;
using System.Collections.Generic;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class FloorTile : FloorTile<Message>
    {
        public FloorTile(FloorInitializer initializer ) : base(initializer) {}
    }

    public class FloorTile<TMessage> : Tile<TMessage> where TMessage : Message 
    {
        public FloorTile(FloorInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(FloorInitializer initializer)
        {
            sides = initializer.Sides;

            initializer.Initializing -= Initialize;
        }

        private IEnumerable<TileSide> sides;
        public override IEnumerable<TileSide> Sides => sides; 

        protected virtual void UpdateWall()
        {
            //TODO
            //wallGraphic.DrawFaces = CubeFaces.All; //reset

            //if (Neighbours.North != null)
            //    wallGraphic.DrawFaces ^= CubeFaces.Back;
            //if (Neighbours.East != null)
            //    wallGraphic.DrawFaces ^= CubeFaces.Right;
            //if (Neighbours.South != null)
            //    wallGraphic.DrawFaces ^= CubeFaces.Front;
            //if (Neighbours.West != null)
            //    wallGraphic.DrawFaces ^= CubeFaces.Left;
        }
        public override bool IsAccessible => true;
    }
}
