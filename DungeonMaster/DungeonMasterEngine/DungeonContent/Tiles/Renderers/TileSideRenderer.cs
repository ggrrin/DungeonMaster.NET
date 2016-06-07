using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public abstract class TileSideRenderer : Renderer
    {
        protected void GetTransformation(MapDirection faces, out Matrix matrix)
        {
            if (faces == MapDirection.North)
            {
                matrix = Matrix.Identity;
                return;
            }

            if (faces == MapDirection.East)
            {
                matrix = Matrix.CreateRotationY(-MathHelper.PiOver2);
                return;
            }

            if (faces == MapDirection.South)
            {
                matrix = Matrix.CreateRotationY(MathHelper.Pi);
                return;
            }

            if (faces == MapDirection.West)
            {
                matrix = Matrix.CreateRotationY(MathHelper.PiOver2);
                return;
            }

            if (faces == MapDirection.Down)
            {
                matrix = Matrix.CreateRotationX(-MathHelper.PiOver2);
                return;
            }

            if (faces == MapDirection.Up)
            {
                matrix = Matrix.CreateRotationX(MathHelper.PiOver2);
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(faces), faces, null);
        }

    }
}