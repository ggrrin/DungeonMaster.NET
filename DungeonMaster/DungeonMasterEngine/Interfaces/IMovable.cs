using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Interfaces
{
    public interface IMovable<U> : ILocalizable<U> where U : IStopable
    {
        Vector3 Position { get; set; }
        float TranslationVelocity { get; }
    }
}