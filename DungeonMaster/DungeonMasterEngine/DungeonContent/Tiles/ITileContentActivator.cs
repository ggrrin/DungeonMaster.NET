namespace DungeonMasterEngine.DungeonContent.Tiles
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
}