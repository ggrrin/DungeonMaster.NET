using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.Helpers
{
    public class BreadthFirstSearch<TTile, TBundle> where TTile : class, INeighbourable<TTile>
    {
        private SearchFabricElement<TTile, TBundle>[][,] processedTiles;

        protected int currentFlag { get; private set; }
        protected TTile originTile { get; private set; }
        private Action<TTile, int, TBundle> action;

        private int dimension => processedTiles[0]?.GetLength(0) ?? 0;
        private int maxRememberedDistance => (dimension - 1) / 2;
        private int maxSpecifedDistance;
        private int maxSpecifiedDimension => (maxSpecifedDistance*2) + 1;

        private readonly Queue<TTile> queue = new Queue<TTile>();
        private int originLevel;
        private bool searchStopped = false;

        public bool Searching { get; private set; } = false;

        public void StartSearch(TTile originTile, TTile startTile, int maxDistance, Action<TTile, int, TBundle> action)
        {
            this.originTile = originTile;
            currentFlag++;
            maxSpecifedDistance = maxDistance;

            if (processedTiles == null || this.maxRememberedDistance <= maxDistance)
            {
                processedTiles = new SearchFabricElement<TTile, TBundle>[3][,];
                for (int i = 0; i < 3; i++)
                    processedTiles[i] = new SearchFabricElement<TTile, TBundle>[2 * maxDistance + 1, 2 * maxDistance + 1];
            }

            this.action = action;
            originLevel = originTile.LevelIndex;

            var startTileDistance = startTile.GridPosition - originTile.GridPosition;
            if (startTileDistance.X > maxDistance || startTileDistance.Y > maxDistance)
                throw new IndexOutOfRangeException("Start Tile is not in searching range");

            Enqueue(startTile, 0, null);
            if (!queue.Any())
                throw new Exception();
            Search();
        }

        private void Search()
        {
            OnSearchStart();
            int layer = 0;
            while (queue.Count > 0)
            {
                TTile currentTile = queue.Dequeue();
                if (currentTile == null)
                    throw new Exception();


                var element = GetElement(currentTile);
                action(currentTile, element.Layer, element.Bundle);

                AddSucessors(++layer, currentTile);

                if (searchStopped)
                {
                    searchStopped = false;
                    queue.Clear();
                    break;
                }
            }

            OnSearchFinished();
        }

        protected virtual void OnSearchStart()
        {
            Searching = true;
        }

        protected virtual void OnSearchFinished()
        {
            Searching = false;
        }

        public void ClearBundles()
        {
            foreach (var layer in processedTiles)
                for (int i = 0; i < layer.GetLength(0); i++)
                    for (int j = 0; j < layer.GetLength(1); j++)
                        layer[i, j].Bundle = default(TBundle);
        }

        protected virtual void AddSucessors(int layer, TTile currentTile)
        {
            foreach (var neighbour in currentTile.Neighbors.Where(x => x.Item2.ShiftType == DirectionShift.VerticalShift))
                Enqueue(neighbour.Item1, layer, currentTile);
        }

        protected void Enqueue(TTile descendant, int layer, TTile previousTile)
        {
            if (descendant != null)
            {
                int? flag = GetFlag(descendant.GridPosition, descendant.LevelIndex);
                if (flag != null && flag != currentFlag)
                {
                    queue.Enqueue(descendant);
                    SetFlag(descendant.GridPosition, descendant.LevelIndex, currentFlag, layer, previousTile);
                }
            }
        }

        public IEnumerable<TTile> GetShortestRouteReverse(TTile destinationTile)
        {
            if (destinationTile == null)
                throw new ArgumentNullException();

            var curElement = GetElement(destinationTile);
            yield return destinationTile;
            while (curElement.PreviousTile != null)
            {
                yield return curElement.PreviousTile;
                curElement = GetElement(curElement.PreviousTile);
            }
        }

        public IReadOnlyList<TTile> GetShortestRoute(TTile destTile)
        {
            if (destTile == null)
                throw new ArgumentNullException();

            var res = GetShortestRouteReverse(destTile).ToArray();
            Array.Reverse(res);
            return res;
        }

        public void StopSearch()
        {
            if (!Searching)
                throw new InvalidOperationException("Searching is not running!");
            searchStopped = true;
        }

        private int? GetFlag(Point pos, int level)
        {
            var relative = GetRelativePos(pos);

            if (relative.X >= 0 && relative.X < maxSpecifiedDimension && relative.Y >= 0 && relative.Y < maxSpecifiedDimension && level >= originLevel - 1 && level <= originLevel + 1)
                return processedTiles[originLevel - level + 1][relative.X, relative.Y].Flag;
            else
                return null;
        }

        private void SetFlag(Point pos, int level, int flag, int layer, TTile previousTile)
        {
            var relative = GetRelativePos(pos);
            processedTiles[originLevel - level + 1][relative.X, relative.Y].Flag = flag;
            processedTiles[originLevel - level + 1][relative.X, relative.Y].Layer = layer;
            processedTiles[originLevel - level + 1][relative.X, relative.Y].PreviousTile = previousTile;
        }

        private Point GetRelativePos(Point pos)
        {
            var relative = pos - originTile.GridPosition;
            relative += new Point(maxSpecifiedDimension/ 2);//shift to natural position
            return relative;
        }

        private SearchFabricElement<TTile, TBundle> GetElement(TTile tile)
        {
            var relative = GetRelativePos(tile.GridPosition);

            var res = processedTiles[originLevel - tile.LevelIndex + 1][relative.X, relative.Y];
            if (res.Flag != currentFlag)
                throw new InvalidOperationException("Invalid reading location. Old data.");
            return res;
        }


        public TBundle GetBundle(TTile tile)
        {
            var relative = GetRelativePos(tile.GridPosition);
            return processedTiles[originLevel - tile.LevelIndex + 1][relative.X, relative.Y].Bundle;
        }

        public void SetBundle(TTile tile, TBundle bundle)
        {
            var relative = GetRelativePos(tile.GridPosition);
            processedTiles[originLevel - tile.LevelIndex + 1][relative.X, relative.Y].Bundle = bundle;
        }

    }
}
