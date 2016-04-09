using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent
{
    public abstract class WorldObject
    {
        public WorldObject(Vector3 position)
        {
            Position = position;
        }

        public abstract IGraphicProvider GraphicsProvider { get; }

        public virtual Vector3 Position { get; set; }
    }
}
