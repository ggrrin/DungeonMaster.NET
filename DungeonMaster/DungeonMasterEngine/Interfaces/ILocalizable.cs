using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Interfaces
{
    public interface ILocalizable<U> where U : IStopable
    {
        U Location { get; set; }
    }

    public interface IMovable<U> : ILocalizable<U> where U : IStopable
    {
        Vector3 Position { get; }
        float TranslationVeloctiy { get; }
    }
}