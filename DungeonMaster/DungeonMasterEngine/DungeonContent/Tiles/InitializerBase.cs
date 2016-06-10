namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public abstract class InitializerBase
    {
        public event Initializer<InitializerBase> Initialized;

        public void Initialize()
        {
            OnInitialize();
            Initialized?.Invoke(this);
        }
        protected abstract void OnInitialize();
    }
}