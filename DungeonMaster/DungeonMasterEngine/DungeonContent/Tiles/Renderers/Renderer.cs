using System;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public abstract class Renderer : IRenderer
    {
        public abstract Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter);

        public abstract Matrix GetCurrentTransformation(ref Matrix parentTransformation);

        public abstract bool Interact(ILeader leader, ref Matrix currentTransformation, object param);

        public virtual void Initialize() { } 

        public bool Highlighted { get; set; }

        public float Epsilon => 0.001f;

        public virtual async void Highlight(int miliseconds)
        {
            Highlighted = true;

            await Task.Delay(miliseconds);

            Highlighted = false;
        }


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