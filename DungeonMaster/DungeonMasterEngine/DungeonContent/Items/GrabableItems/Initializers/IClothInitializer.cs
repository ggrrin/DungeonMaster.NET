namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers
{
    public interface IClothInitializer
    {
        bool IsBroken { get; }
        bool IsCruised { get; }
    }
}