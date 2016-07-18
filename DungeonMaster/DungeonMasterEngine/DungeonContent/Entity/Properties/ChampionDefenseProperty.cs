namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class ChampionDefenseProperty : DefenseProperty
    {
        private readonly ILiveEntity entity;

        public ChampionDefenseProperty(ILiveEntity entity, int value) : base(value)
        {
            this.entity = entity;
        }
    }
}