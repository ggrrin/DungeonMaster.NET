using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface IInteractor
    {
        void Interact(ILeader leader, ref Matrix matrix, object param);
        void Initialize();
    }

    public abstract class Interactor : IInteractor
    {
        public abstract void Interact(ILeader leader, ref Matrix matrix, object param);

        public virtual void Initialize() { }
    }
}