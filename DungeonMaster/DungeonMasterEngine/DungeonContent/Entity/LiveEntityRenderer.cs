using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    class LiveEntityRenderer<TEntity> : Renderer where TEntity : ILiveEntity
    {
        protected readonly CubeGraphic cube;
        public TEntity Entity { get; }

        public LiveEntityRenderer(TEntity entity, Texture2D face)
        {
            Entity = entity;

            cube = new CubeGraphic
            {
                Texture = face,
                Position = Entity.Position,
                DrawFaces = CubeFaces.All,
                Outter = true,
                Scale = new Vector3(0.1f,0.3f, 0.1f),
            };
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            cube.Position = Entity.Position;
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