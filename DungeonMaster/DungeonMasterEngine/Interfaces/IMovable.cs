using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Interfaces
{
    public interface IMovable<U> : ILocalizable<U> where U : IStopable
    {
        Vector3 Position { get; set; }
        float TranslationVelocity { get; }

        void MoveTo(ITile newLocation, bool setNewLocation);
        void Update(GameTime time);
    }
}