using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;

namespace DungeonMasterParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var dat = new DungeonParser();
            dat.Parse();

            a(dat.Data.Maps[1], dat.Data);

            using (var w = new StreamWriter("output.txt"))
            {

                //var stairTiles = (from i in dat.Data.Maps.SelectMany(x => x.Tiles) where i.GetType() == typeof(StairsTile) select i).SelectMany(x => x.GetItems(dat.Data));


                //var q = from j in stairTiles

                //        where j.GetType() == typeof(Actuator) //&& (j as Actuator).AcutorType == 5/*stair actuator*/
                //        select new { Action = ((Actuator)j).Action, Position = ((Actuator)j).ActionType.ToString() };

                //foreach (var k in from i in dat.Data.ObjectIDs
                //                  where i.Category == ObjectCategory.Actuators && ((Actuator)i.GetObject(dat.Data)).AcutorType == 5
                //                  select ((RmtTrg)((Actuator)i.GetObject(dat.Data)).ActionType).Position.ToString() + " " +
                //                  dat.Data.Actuators.IndexOf((Actuator)i.GetObject(dat.Data)))

                //    w.WriteLine(k);


                //var stairAcutorsIDs = (from i in new int[] { 55, 166, 402, 166, 127 }
                //                       select ((((0 ^ (int)TilePosition.North_TopLeft) << 4) ^ (int)ObjectCategory.Actuators) << 10) ^ i).ToArray();
                WriteActuators(w, dat);
                WriteStairTiles(w, dat);
                WriteMapsTiles(w, dat);
            }
        }

        private static void WriteActuators(StreamWriter w, DungeonParser dat)
        {
            bool line = false;
            for (int i = 0; i < dat.Data.Maps.Count; i++)
            {
                for (int x = 0; x < dat.Data.Maps[i].Width; x++)
                {
                    for (int y = 0; y < dat.Data.Maps[i].Height; y++)
                    {
                        var tile = dat.Data.Maps[i][dat.Data.Maps[i].OffsetX + x, dat.Data.Maps[i].OffsetY + y];

                        if (line)
                        {
                            w.WriteLine("0000000000000000000000000000000000000000000000000000000000");
                            line = false;
                        }

                        //var door = tile as DoorTileData;
                        //if (door != null)
                        //{
                        //    w.WriteLine($"state:{door.State} {x + dat.Data.Maps[i].OffsetX} {y + dat.Data.Maps[i].OffsetY}; Level: {i}");
                        //}

                        foreach (ActuatorItemData a in tile.GetItems(dat.Data).OfType<ActuatorItemData>().Where(xz => /*xz.Action == ActionType.Hold &&*/ !xz.IsLocal &&
                       dat.Data.Maps[i][dat.Data.Maps[i].OffsetX + ((RmtTrg)xz.ActLoc).Position.Position.X, dat.Data.Maps[i].OffsetY + ((RmtTrg)xz.ActLoc).Position.Position.Y] is PitTileData))
                        {
                            //if (tile.GetType() != typeof(WallTile) || !(a.AcutorType == 5 || a.AcutorType == 6 ) )
                            //    continue;

                            w.WriteLine($"AbsolutePositon: {x + dat.Data.Maps[i].OffsetX} {y + dat.Data.Maps[i].OffsetY}; Level: {i}; is wall {tile is WallTileData} ");
                            ObjectDumper.Write(a, 2, w);
                            w.WriteLine();
                            line = true;
                        }
                    }
                }
            }
        }


        private static void a(DungeonMap map, DungeonData data)
        {
            //from i in map.Tiles.SelectMany(x => x.GetItems(data)) where i.GetType() == typeof(WeaponItem) && ((WeaponItem)i).ItemTypeIndex == 9 select i
            for (int i = 0; i < map.Tiles.Count; i++)
            {
                foreach (var k in map.Tiles[i].GetItems(data))
                    if (k.GetType() == typeof(WeaponItemData) && ((WeaponItemData)k).ItemTypeIndex == 9)
                    {
                        Console.WriteLine("{0} | {1} {2} ", i, i / map.Height, i % map.Height);
                        break;
                    }
            }
        }

        private static void WriteStairTiles(StreamWriter w, DungeonParser dat)
        {
            var lst = new List<string>();

            for (int i = 0; i < dat.Data.Maps.Count; i++)
            {
                for (int x = 0; x < dat.Data.Maps[i].Width; x++)
                {
                    for (int y = 0; y < dat.Data.Maps[i].Height; y++)
                    {
                        var tile = dat.Data.Maps[i][dat.Data.Maps[i].OffsetX + x, dat.Data.Maps[i].OffsetY + y];
                        if (tile?.GetType() == typeof(StairsTileData))
                        {
                            lst.Add($"AbsolutePositon: {x + dat.Data.Maps[i].OffsetX} {y + dat.Data.Maps[i].OffsetY}; Level: {i};  orientation: {(tile as StairsTileData).Orientation}; vertical : {(tile as StairsTileData).Direction};");
                        }

                    }
                }
            }

            lst.Sort();

            foreach (var s in lst)
                w.WriteLine(s);

        }

        private static void WriteMapsTiles(StreamWriter w, DungeonParser dat)
        {
            int mc = 0;
            foreach (var m in dat.Data.Maps)
            {
                w.WriteLine(mc++);
                w.WriteLine($"{m.OffsetY} {m.OffsetX}");
                for (int i = 0; i < m.Tiles.Count; i++)
                {

                    if (i % m.Height == 0)
                    {
                        w.WriteLine();
                        for (int s = 0; s < m.OffsetY; s++)
                            w.Write(" ");

                    }



                    var letter = m.Tiles[i].GetType().ToString().Split(new char[] { '.' })[1].Substring(0, 1);

                    switch (letter)
                    {
                        case "F":
                            w.Write(" ");

                            break;
                        case "W":
                            w.Write("■");
                            break;
                        default:
                            w.Write(letter);
                            break;
                    }
                }

                w.WriteLine();
            }

        }
    }
}
