namespace DungeonMasterEngine.DungeonContent.Tiles
{
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