namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public delegate void Initializer<TObject>(TObject initializer) where TObject : InitializerBase;
}