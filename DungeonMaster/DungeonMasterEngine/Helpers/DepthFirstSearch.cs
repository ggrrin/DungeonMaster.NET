using Microsoft.Xna.Framework;
using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.Helpers
{
    struct DepthFirstSearch
    {
        private double[,] onStack;
        private double currentTimeFlag;
        private ITile startTile;
        private Action<ITile> action;

        private int dimension => onStack.GetLength(0);

        public void StartSearch(ITile startTile, double currentTimeFlag, int dimension, Action<ITile> action)
        {
            this.startTile = startTile;
            this.currentTimeFlag = currentTimeFlag;

            if (onStack == null || onStack.GetLength(0) != dimension)
                onStack = new double[dimension, dimension];

            this.action = action;

            Search(startTile);
        }


        private void Search(ITile currentTile)
        {
            action(currentTile);
            this[currentTile.GridPosition] = currentTimeFlag;

            SolveDescendant(currentTile.Neighbors.North);
            SolveDescendant(currentTile.Neighbors.West);
            SolveDescendant(currentTile.Neighbors.East);
            SolveDescendant(currentTile.Neighbors.South);
        }

        private void SolveDescendant(ITile descendant)
        {
            if (descendant != null && this[descendant.GridPosition] != null && this[descendant.GridPosition] != currentTimeFlag)
                Search(descendant);
        }


        private double? this[Point pos]
        {
            get
            {
                var relative = GetRelativePos(pos);

                if (relative.X >= 0 && relative.X < dimension && relative.Y >= 0 && relative.Y < dimension)
                    return onStack[relative.X, relative.Y];
                else
                    return null;
            }

            set
            {
                if (!value.HasValue) throw new ArgumentNullException();

                var relative = GetRelativePos(pos);

                onStack[relative.X, relative.Y] = value.Value;
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
