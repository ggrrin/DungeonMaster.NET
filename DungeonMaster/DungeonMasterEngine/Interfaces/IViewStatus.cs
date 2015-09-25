using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Interfaces
{
    public interface IViewStatus
    {
        Matrix View { get;  }
        Matrix Projection { get;  }
    }
}