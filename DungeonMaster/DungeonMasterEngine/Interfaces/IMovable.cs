using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Interfaces
{
    public interface IMovable<U> : ILocalizable<U>, IUpdate where U : IStopable
    {
        Vector3 Position { get; set; }
        float TranslationVelocity { get; }

        void MoveTo(ITile newLocation, bool setNewLocation);
    }
}