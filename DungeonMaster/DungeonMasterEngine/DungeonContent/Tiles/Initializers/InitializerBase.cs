namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public abstract class InitializerBase
    {
        public void Initialize()
        {
            OnInitialing();
            OnInitialized();
        }

        protected abstract void OnInitialing();

        protected abstract void OnInitialized();
    }
}