namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public delegate void Initializer<TObject>(TObject initializer) where TObject : InitializerBase;
}