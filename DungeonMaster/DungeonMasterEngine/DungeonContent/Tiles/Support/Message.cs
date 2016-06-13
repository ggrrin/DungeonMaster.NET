namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Message
    {
        public MapDirection Specifier { get; }
        public MessageAction Action { get; }

        public Message(MessageAction action, MapDirection specifier)
        {
            Action = action;
            Specifier = specifier;
        }
    }
}