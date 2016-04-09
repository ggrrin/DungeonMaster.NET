using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.Helpers
{
    class BreadthFirstSearch
    {
        private int[][,] processedTiles;

        private int currentFlag;
        private Tile startTile;
        private Action<Tile> action;
        private int dimension { get { var res = processedTiles[0]?.GetLength(0); if (res.HasValue) return res.Value; else return 0; } }

        private Queue<Tile> queue = new Queue<Tile>();
        private int startLevel;

        public void StartSearch(Tile startTile, int dimension, Action<Tile> action)
        {
            this.startTile = startTile;
            currentFlag++;

            if (processedTiles == null || this.dimension != dimension)
            {
                processedTiles = new int[3][,];
                for (int i = 0; i < 3; i++)
                    processedTiles[i] = new int[dimension, dimension];
            }

            this.action = action;
            startLevel = startTile.LevelIndex;
            Enqueue(startTile);
            Search();
        }


        private void Search()
        {

            while (queue.Count > 0)
            {
                Tile currentTile = queue.Dequeue();
                action(currentTile);

                Enqueue(currentTile.Neighbours.North);
                Enqueue(currentTile.Neighbours.West);
                Enqueue(currentTile.Neighbours.East);
                Enqueue(currentTile.Neighbours.South);
            }
        }

        private void Enqueue(Tile descendant)
        {
            if (descendant != null && this[descendant.GridPosition, descendant.LevelIndex] != null && this[descendant.GridPosition, descendant.LevelIndex] != currentFlag)
            {
                queue.Enqueue(descendant);
                this[descendant.GridPosition, descendant.LevelIndex] = currentFlag;
            }
        }


        private int? this[Point pos, int level]
        {
            get
            {
                var relative = GetRelativePos(pos);

                if (relative.X >= 0 && relative.X < dimension && relative.Y >= 0 && relative.Y < dimension && level >= startLevel - 1 && level <= startLevel + 1)
                    return processedTiles[startLevel - level + 1][relative.X, relative.Y];
                else
                    return null;
            }

            set
            {
                if (!value.HasValue) throw new ArgumentNullException();

                var relative = GetRelativePos(pos);

                processedTiles[startLevel - level + 1][relative.X, relative.Y] = value.Value;
            }
        }

        private Point GetRelativePos(Point pos)
        {
            var relative = pos - startTile.GridPosition;
            relative += new Point(dimension / 2);//shift to natural position
            return relative;
        }

    }


}
