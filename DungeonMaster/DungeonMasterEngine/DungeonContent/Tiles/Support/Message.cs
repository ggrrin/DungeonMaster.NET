namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Message
    {
        public int Specifier { get; }
        public MessageAction Action { get; }

        public Message(MessageAction action, int specifier)
        {
            Action = action;
            Specifier = specifier;
        }
    }
}