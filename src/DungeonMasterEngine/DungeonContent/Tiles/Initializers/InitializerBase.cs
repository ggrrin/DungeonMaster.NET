namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public abstract class InitializerBase
    {
        public void Initialize()
        {
            OnInitializing();
        }

        public void NotifyInitialized()
        {
            OnInitialized();
        }

        protected abstract void OnInitializing();

        protected abstract void OnInitialized();
    }
}