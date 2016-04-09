using System.Collections;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent
{

    public class Neighbours : INeighbours
    {
        public Neighbours()
        { }

        public Neighbours(INeighbours neigbours)
        {
            North = neigbours.North;
            South = neigbours.South;
            East = neigbours.East;
            West = neigbours.West;
        }

        public virtual Tile North { get; set; }
        public virtual Tile South { get; set; }
        public virtual Tile East { get; set; }
        public virtual Tile West { get; set; }

        public Tile GetTile(Point shift)
        {
            if (shift == new Point(1, 0))
                return East;
            else if (shift == new Point(-1, 0))
                return West;
            else if (shift == new Point(0, -1))
                return North;
            else if (shift == new Point(0, 1))
                return South;
            else
                return null;
        }

        public override string ToString()
        {
            return $"North: {North} South: {South} East: {East} West: {West}";
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            if (North != null)
                yield return North;
            if (East != null)
                yield return East;

            if (South != null)
                yield return South;

            if (West != null)
                yield return West;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (North != null)
                yield return North;
            if (East != null)
                yield return East;

            if (South != null)
                yield return South;

            if (West != null)
                yield return West;
        }

        IEnumerator<KeyValuePair<Tile, Point>> IEnumerable<KeyValuePair<Tile, Point>>.GetEnumerator()
        {
            yield return new KeyValuePair<Tile, Point>(North, new Point(0, -1));
            yield return new KeyValuePair<Tile, Point>(East, new Point(1, 0));
            yield return new KeyValuePair<Tile, Point>(South, new Point(0, 1));
            yield return new KeyValuePair<Tile, Point>(West, new Point(-1, 0));
        }
    }
}