using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent
{
    public interface IWorldObject : IRenderable
    {
        Vector3 Position { get; }
    }
}