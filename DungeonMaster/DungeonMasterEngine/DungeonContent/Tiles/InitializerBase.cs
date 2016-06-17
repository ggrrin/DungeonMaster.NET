namespace DungeonMasterEngine.DungeonContent.Tiles
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