using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent
{
    public interface IWorldObject
    {
        IGraphicProvider GraphicsProvider { get; }
        Vector3 Position { get; set; }
    }
}