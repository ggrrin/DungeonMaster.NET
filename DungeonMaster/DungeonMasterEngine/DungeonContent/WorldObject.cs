using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent
{
    public abstract class WorldObject : IWorldObject
    {
        public abstract IGraphicProvider GraphicsProvider { get; }

        public virtual Vector3 Position { get; set; }
        public IRenderer Renderer { get; set; }
    }
}
