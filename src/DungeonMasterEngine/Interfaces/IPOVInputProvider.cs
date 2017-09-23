using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.Interfaces
{
    public interface IPOVInputProvider
    {
        WalkDirection CurrentDirection { get; }
    }
}