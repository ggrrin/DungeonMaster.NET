namespace DungeonMasterEngine.Interfaces
{
    public enum WalkDirection
    {
        None, Forward, Backward, Left, Right
    }

    public interface IPOVInputProvider
    {
        WalkDirection CurrentDirection { get; }
    }
}