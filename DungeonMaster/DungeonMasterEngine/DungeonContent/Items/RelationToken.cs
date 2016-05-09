namespace DungeonMasterEngine.DungeonContent.Items
{
    public struct RelationToken
    {
        private readonly uint Token;

        public RelationToken(uint token)
        {
            Token = token;
        }

        public override bool Equals(object obj)
        {
            if (obj is RelationToken)
                return Token.Equals(((RelationToken) obj).Token);

            return false;
        }

        public static bool operator ==(RelationToken l, RelationToken r) => l.Equals(r);

        public static bool operator !=(RelationToken l, RelationToken r) => !(l == r);

        public override int GetHashCode() => Token.GetHashCode();
    }
}