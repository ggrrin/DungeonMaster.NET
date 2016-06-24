using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.Helpers
{
    public class RendererSearcher : BreadthFirstSearch<ITile, object>
    {
        protected override void AddSucessors(int layer, ITile currentTile)
        {
            foreach (var neighbour in currentTile.Neighbours)
                Enqueue(neighbour.Item1, layer, currentTile);
        }
    }
}