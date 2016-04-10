using DungeonMasterEngine.GameConsoleContent;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Support;

namespace DungeonMasterEngine.Helpers
{
    public static class ExtensionHelper
    {
        public static Point ToAbsolutePosition(this Position p, DungeonMasterParser.DungeonMap map)
        {
            Point absolutePosition = new Point();
            absolutePosition.X = map.OffsetX + p.X;
            absolutePosition.Y = map.OffsetY + p.Y;
            return absolutePosition;
        }


        public static DungeonMasterParser.Tiles.Tile GetTile(this DungeonMasterParser.DungeonMap map, Point x) => map[x.X, x.Y];

        public static IEnumerable<T> ReverseLazy<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
                yield return list[list.Count - (i + 1)];
        }


        public static Vector3 ToGridVector3(this Point p, int level)
        {
            return new Vector3(p.X, -level, p.Y);
        }

        public static TilePosition ToDirection(this Point p)
        {
            if (p == new Point(0, -1))
                return TilePosition.North_TopLeft;
            else if (p == new Point(1, 0))
                return TilePosition.East_TopRight;
            else if (p == new Point(0, 1))
                return TilePosition.South_BottomLeft;
            else if (p == new Point(-1, 0))
                return TilePosition.West_BottomRight;
            else
                throw new ArgumentException();
        }

        public static Point ToGrid(this Vector3 p)
        {
            return new Point(LeastWholeNumber(p.X), LeastWholeNumber(p.Z));
        }

        private static int LeastWholeNumber(float p)
        {
            int x;
            if (p < 0 && p != (int)p)
                x = (int)p - 1;
            else
                x = (int)p;

            return x;
        }

        public static void Dump(this object o, int depth = 0)
        {
            ObjectDumper.Write(o, depth, GameConsole.Instance?.Out);
        }

        public static void Dump(this string o, int depth = 0)
        {
            GameConsole.Instance?.Out.WriteLine(o);
        }

        public static string DumpString(this object o)
        {
            return ObjectDumper.Dump(o);
        }



        //var q = from item in wall.GetItems(data).OrderBy(x => x, Comparer<SuperItem>.Create((x, y) =>
        //{
        //    bool xAct = x.GetType() == typeof(ActuatorItem);
        //    bool yAct = y.GetType() == typeof(ActuatorItem);
        //    if (xAct && yAct)
        //        return -((ActuatorItem)x).AcutorType.CompareTo(((ActuatorItem)y).AcutorType);
        //    else if (xAct && !yAct)
        //        return -1;
        //    else if (!xAct && yAct)
        //        return 1;
        //    else
        //        return 0;
        //}))
        //        select item;

    }
}
