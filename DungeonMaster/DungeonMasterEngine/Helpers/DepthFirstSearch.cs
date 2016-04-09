using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.Helpers
{
    struct DepthFirstSearch
    {
        private double[,] onStack;
        private double currentTimeFlag;
        private Tile startTile;
        private Action<Tile> action;
        private int dimension { get { return onStack.GetLength(0); } }

        public void StartSearch(Tile startTile, double currentTimeFlag, int dimension, Action<Tile> action)
        {
            this.startTile = startTile;
            this.currentTimeFlag = currentTimeFlag;

            if (onStack == null || onStack.GetLength(0) != dimension)
                onStack = new double[dimension, dimension];

            this.action = action;

            Search(startTile);
        }


        private void Search(Tile currentTile)
        {
            action(currentTile);
            this[currentTile.GridPosition] = currentTimeFlag;

            SolveDescendant(currentTile.Neighbours.North);
            SolveDescendant(currentTile.Neighbours.West);
            SolveDescendant(currentTile.Neighbours.East);
            SolveDescendant(currentTile.Neighbours.South);
        }

        private void SolveDescendant(Tile descendant)
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
