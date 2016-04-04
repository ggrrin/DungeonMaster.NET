using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Tiles;
using DungeonMasterParser;

namespace DungeonMasterEngine
{
    public interface ITileContentActivator
    {
        void ActivateContent(Gateway t);
        void ActivateContent(Floor t);
        void ActivateContent(Pit t);
        void ActivateContent(Stairs t);
        void ActivateContent(Teleport t);
        void ActivateContent(WallIlusion t);
        void ActivateContent(LogicTile t);
    }

    public class TileContentActivator : ITileContentActivator
    {
        public virtual void ActivateContent(Gateway t){}
        public virtual void ActivateContent(Floor t){}
        public virtual void ActivateContent(Pit t){}
        public virtual void ActivateContent(Stairs t){}
        public virtual void ActivateContent(Teleport t){}
        public virtual void ActivateContent(WallIlusion t){}
        public virtual void ActivateContent(LogicTile t){}
    }
}