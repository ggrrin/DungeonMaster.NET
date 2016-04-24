using DungeonMasterEngine.GameConsoleContent;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterParser;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using DungeonMasterEngine.DungeonContent.Actuators.Floor;
using DungeonMasterParser.Items;
using DungeonMasterEngine.DungeonContent.Actuators;

namespace DungeonMasterEngine.Helpers
{
    public static class ExtensionHelper
    {

        public static ActionStateX GetActionStateX(this ActuatorItemData actuator)
        {
            int specifer = -1;
            var location = actuator.ActLoc as RmtTrg;
            if (location != null)
                specifer = (int)location.Position.Direction;

            return new ActionStateX((ActionState)actuator.Action, specifer);
        }

        public static Point ToAbsolutePosition(this Position p, DungeonMap map)
        {
            Point absolutePosition = new Point
            {
                X = map.OffsetX + p.X,
                Y = map.OffsetY + p.Y
            };
            return absolutePosition;
        }

        /// <summary>
        /// If source is null returns true. Otherwise returns whether source value is equal to param.
        /// </summary>
        /// <typeparam name="TSoutce"></typeparam>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="source"></param>
        /// <param name="param"></param>
        /// <returns>If source is null returns true. Otherwise returns whether source value is equal to param.</returns>
        public static bool OptionalyEquals<TSoutce, TParam>(this TSoutce? source, TParam param) where  TSoutce : struct 
        {
            if (source.HasValue)
            {
                return source.Value.Equals(param);
            }
            else
            {
                return true;
            }
        }


        public static bool SequenceSimilar<TSource, TSimilar>(this IEnumerable<TSource> source, IEnumerable<TSimilar> similar) where  TSource : IEquatable<TSimilar>
        {
            if (source == null)
                throw  new ArgumentNullException();
            if (similar == null)
                throw new ArgumentNullException(); 

            using (var e1 = source.GetEnumerator())
            using (var e2 = similar.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (!(e2.MoveNext() && e1.Current.Equals(e2.Current))) return false;
                }
                if (e2.MoveNext()) return false;
            }
            return true;
        }


        public static TileData GetTileData(this DungeonMap map, Point x) => map[x.X, x.Y];

        public static IEnumerable<T> ReverseLazy<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
                yield return list[list.Count - (i + 1)];
        }


        public static Vector3 ToGridVector3(this Point p, int level)
        {
            return new Vector3(p.X, -level, p.Y);
        }

        public static MapDirection ToMapDirection(this Point p)
        {
            if (p == new Point(0, -1))
                return MapDirection.North;
            else if (p == new Point(1, 0))
                return MapDirection.East;
            else if (p == new Point(0, 1))
                return MapDirection.South;
            else if (p == new Point(-1, 0))
                return MapDirection.West;
            else
                throw new ArgumentException();
        }

        public static TilePosition ToTilePosition(this Point p)
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
