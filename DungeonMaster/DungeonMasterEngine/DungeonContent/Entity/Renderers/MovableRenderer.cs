using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Entity.Renderers
{
    class MovableRenderer<TMovable> : Renderer where TMovable : IMovable<ISpaceRouteElement>
    {
        protected readonly CubeGraphic cube;
        public TMovable Movable { get; }

        public MovableRenderer(TMovable movable, Texture2D face)
        {
            Movable = movable;

            cube = new CubeGraphic
            {
                Texture = face,
                Position = Movable.Position,
                DrawFaces = CubeFaces.All,
                Outter = true,
                Scale = new Vector3(0.1f, 0.3f, 0.1f),
            };
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            cube.Position = Movable.Position;
            cube.Draw(effect);
            return currentTransformation;
        }

        public override Matrix GetCurrentTransformation(ref Matrix parentTransformation)
        {
            return Matrix.Identity;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            return false;
        }
    }
}