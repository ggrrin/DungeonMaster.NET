using DungeonMasterEngine.GameConsoleContent;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterParser;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using DungeonMasterParser.Items;
using DungeonMasterEngine.DungeonContent.Actuators;

namespace DungeonMasterEngine.Helpers
{
    public static class ExtensionHelper
    {
        public static TSource MinObj<TSource>(this IEnumerable<TSource> source, Func<TSource, float> getValue)
        {
            var min = Tuple.Create(default(TSource), float.MaxValue);
            foreach (var t in source.Select(e => Tuple.Create(e, getValue(e))))
            {
                if (t.Item2 < min.Item2)
                    min = t;
            }
            return min.Item1;
        }

        public static MapDirection ToMapDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return MapDirection.North;
                case Direction.East:
                    return MapDirection.East;
                case Direction.South:
                    return MapDirection.South;
                case Direction.West:
                    return MapDirection.West;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static IEnumerable<MapDirection> ToDirections(this TilePosition x)
        {
            switch (x)
            {
                case TilePosition.North_TopLeft:
                    return new[]
                    {
                        MapDirection.West,
                        MapDirection.North,
                    };
                case TilePosition.East_TopRight:
                    return new[]
                    {
                        MapDirection.East,
                        MapDirection.North,
                    };
                case TilePosition.South_BottomLeft:
                    return new[]
                    {
                        MapDirection.West,
                        MapDirection.South,
                    };
                case TilePosition.West_BottomRight:
                    return new[]
                    {
                        MapDirection.East,
                        MapDirection.South,
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(x), x, null);
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(this T val)
        {
            return Enumerable.Repeat(val, 1);
        }

        public static Point GetParentPosition(this TextDataItem tag, Point parentDataPosition)
        {
            Point relativePos;
            switch (tag.TilePosition)
            {
                case TilePosition.North_TopLeft:
                    relativePos = new Point
                    {
                        Y = -1
                    };
                    break;
                case TilePosition.East_TopRight:
                    relativePos = new Point
                    {
                        X = 1
                    };
                    break;
                case TilePosition.South_BottomLeft:
                    relativePos = new Point
                    {
                        Y = 1
                    };
                    break;
                case TilePosition.West_BottomRight:
                    relativePos = new Point
                    {
                        X = -1
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return parentDataPosition + relativePos;
        }

        public static ActionStateX GetActionStateX(this ActuatorItemData actuator)
        {
            int specifer = -1;
            var location = actuator.ActionLocation as RemoteTarget;
            if (location != null)
                specifer = (int)location.Position.Direction;

            return new ActionStateX((ActionState)actuator.Action, actuator.ActionDelay * 1000 / 6, actuator.IsOnceOnly, specifer);
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
        public static bool OptionalyEquals<TSoutce, TParam>(this TSoutce? source, TParam param) where TSoutce : struct
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

        public static bool SequenceSimilar<TSource, TSimilar>(this IEnumerable<TSource> source, IEnumerable<TSimilar> similar) where TSource : IEquatable<TSimilar>
        {
            if (source == null)
                throw new ArgumentNullException();
            if (similar == null)
                throw new ArgumentNullException();

            using (var e1 = source.GetEnumerator())
            using (var e2 = similar.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (!(e2.MoveNext() && e1.Current.Equals(e2.Current)))
                        return false;
                }
                if (e2.MoveNext())
                    return false;
            }
            return true;
        }

        public static TileData GetTileData(this DungeonMap map, Point x) => map[x.X, x.Y];

        public static IEnumerable<T> ReverseLazy<T>(this IList<T> list)
        {
            return list.Select((t, i) => list[list.Count - (i + 1)]);
        }

        public static Vector3 ToGridVector3(this Point p, int level)
        {
            return new Vector3(p.X, -level, p.Y);
        }

        public static TilePosition ToTilePosition(this MapDirection mapDirection)
        {
            if (mapDirection == MapDirection.North)
                return TilePosition.North_TopLeft;
            else if (mapDirection == MapDirection.East)
                return TilePosition.East_TopRight;
            else if (mapDirection == MapDirection.South)
                return TilePosition.South_BottomLeft;
            else if (mapDirection == MapDirection.West)
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
    }
}
