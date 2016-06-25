namespace DungeonMasterEngine.DungeonContent.GrabableItems.Initializers
{
    public interface IClothInitializer
    {
        bool IsBroken { get; }
        bool IsCruised { get; }
    }
}