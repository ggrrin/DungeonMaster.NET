namespace DungeonMasterEngine.DungeonContent.Entity.Relations
{
    public class RelationTokenFactory
    {
        private uint counter;

        public static RelationTokenFactory Instance { get; } = new RelationTokenFactory();

        public static RelationToken GetNextToken() => Instance.NextToken;

        private RelationToken NextToken => new RelationToken(counter++);

        public RelationTokenFactory()
        {
            counter = 0;
        }


    }
}