using DungeonMasterEngine.DungeonContent;

namespace DungeonMasterEngine.Interfaces
{
    public interface ILocalizable<U> where U : IStopable
    {
        U Location { get; set; }
        MapDirection MapDirection { get; set; }
    }
}