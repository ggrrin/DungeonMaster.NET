using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Builders;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.Helpers
{
    class BuilderMocap : IDungonBuilder
    {
        public DungeonLevel GetLevel(int i, Dungeon dungeon, Point? startTile)
        {
            var tiles = new List<Tile>();

            if (i == 0)
            {
                tiles.Add(new Floor(new Vector3(0, 0, 2)));

                tiles.Add(new Stairs(new Vector3(2, 0, -3), false, true));
                tiles.Add(new Floor(new Vector3(2, 0, -2)));
                tiles.Add(new Floor(new Vector3(3, 0, -2)));
                tiles.Add(new Floor(new Vector3(3, 0, -1)));
                tiles.Add(new Floor(new Vector3(3, 0, 0)));
                tiles.Add(new Floor(new Vector3(4, 0, 0)));
                tiles.Add(new Floor(new Vector3(5, 0, 0)));
                tiles.Add(new Floor(new Vector3(6, 0, 0)));
                tiles.Add(new Floor(new Vector3(7, 0, 0)));
                tiles.Add(new Floor(new Vector3(8, 0, 0)));
                tiles.Add(new Floor(new Vector3(6, 0, -1)));
                tiles.Add(new Floor(new Vector3(6, 0, -2)));
                //Tiles.Add(new Floor(new Vector3(0, 0, 0), CubeFaces.Sides ^ CubeFaces.Back ^ CubeFaces.Left));
                tiles.Add(new Stairs(new Vector3(0, 0, 1), false, false));
                tiles.Add(new Floor(new Vector3(-1, 0, 2) ));
                tiles.Add(new Stairs(new Vector3(-2, 0, 2), true, true));
                tiles.Add(new Stairs(new Vector3(5, 0, -2), true, false));
            }
            else
            {
                tiles.Add(new Stairs(new Vector3(2, -1, -3)));
                tiles.Add(new Floor(new Vector3(1, -1, -3)));
                tiles.Add(new Floor(new Vector3(1, -1, -2)));
                tiles.Add(new Floor(new Vector3(1, -1, -1)));
                tiles.Add(new Floor(new Vector3(1, -1, 0)));
                tiles.Add(new Floor(new Vector3(0, -1, -1)));
                tiles.Add(new Floor(new Vector3(-1, -1, -1)));
                tiles.Add(new Floor(new Vector3(0, -1, 0)));

                tiles.Add(new Stairs(new Vector3(0, -1, 1)));
                tiles.Add(new Floor(new Vector3(-2, -1, -1)));

                tiles.Add(new Floor(new Vector3(-2, -1, 0)));
                tiles.Add(new Floor(new Vector3(-2, -1, 1)));
                tiles.Add(new Stairs(new Vector3(-2, -1, 2)));
                tiles.Add(new Floor(new Vector3(2, -1, 0)));
                tiles.Add(new Floor(new Vector3(3, -1, 0)));
                tiles.Add(new Floor(new Vector3(4, -1, 0)));
                tiles.Add(new Floor(new Vector3(4, -1, -1)));
                tiles.Add(new Floor(new Vector3(4, -1, -2)));
                tiles.Add(new Stairs(new Vector3(5, -1, -2)));
            }

            //if (i == 0)
            //{
            //    Tiles.Add(new Floor(new Vector3(0, 0, 2), CubeFaces.Sides ^ CubeFaces.Back ^ CubeFaces.Left));

            //    Tiles.Add(new Stairs(new Vector3(0, 0, -3), false, true));
            //    Tiles.Add(new Floor(new Vector3(0, 0, -2), CubeFaces.Sides ^ CubeFaces.Back  ^ CubeFaces.Right));
            //    Tiles.Add(new Floor(new Vector3(1, 0, -2), CubeFaces.Sides ^ CubeFaces.Left ^ CubeFaces.Right));
            //    //Tiles.Add(new Floor(new Vector3(0, 0, 0), CubeFaces.Sides ^ CubeFaces.Back ^ CubeFaces.Left));
            //    Tiles.Add(new Stairs(new Vector3(0, 0, 1), false, false));
            //    Tiles.Add(new Floor(new Vector3(-1, 0, 2), CubeFaces.Sides ^ CubeFaces.Right ^ CubeFaces.Left));
            //    Tiles.Add(new Stairs(new Vector3(-2, 0, 2), true, true));
            //}
            //else
            //{
            //    Tiles.Add(new Stairs(new Vector3(0, -1, -3)));
            //    Tiles.Add(new Floor(new Vector3(1, -1, -3), CubeFaces.Sides ^ CubeFaces.Left ^ CubeFaces.Front));
            //    Tiles.Add(new Floor(new Vector3(1, -1, -2), CubeFaces.Sides ^ CubeFaces.Front ^ CubeFaces.Back));
            //    Tiles.Add(new Floor(new Vector3(1, -1, -1), CubeFaces.Sides ^ CubeFaces.Left ^ CubeFaces.Front ^ CubeFaces.Back));
            //    Tiles.Add(new Floor(new Vector3(1, -1, 0), CubeFaces.Sides ^ CubeFaces.Left ^ CubeFaces.Back));
            //    Tiles.Add(new Floor(new Vector3(0, -1, -1), CubeFaces.Sides ^ CubeFaces.Left ^ CubeFaces.Right ^ CubeFaces.Front));
            //    Tiles.Add(new Floor(new Vector3(-1, -1, -1), CubeFaces.Sides ^ CubeFaces.Left ^ CubeFaces.Right ));
            //    Tiles.Add(new Floor(new Vector3(0, -1, 0), CubeFaces.Sides ^ CubeFaces.Front ^ CubeFaces.Back ^ CubeFaces.Right));

            //    Tiles.Add(new Stairs(new Vector3(0, -1, 1)));
            //    Tiles.Add(new Floor(new Vector3(-2, -1, -1), CubeFaces.Sides ^ CubeFaces.Right ^ CubeFaces.Front));

            //    Tiles.Add(new Floor(new Vector3(-2, -1, 0), CubeFaces.Sides ^ CubeFaces.Back ^ CubeFaces.Front));
            //    Tiles.Add(new Floor(new Vector3(-2, -1, 1), CubeFaces.Sides ^ CubeFaces.Back ^ CubeFaces.Front));
            //    Tiles.Add(new Stairs(new Vector3(-2, -1, 2)));
            //}

            var tilePositions = new Dictionary<Point, Tile>();

            foreach (var t in tiles)            
                tilePositions.Add(t.GridPosition, t);

            OldDungeonBuilder.SetupNeighbours(tilePositions, tiles);

            return new DungeonLevel(dungeon, tiles, i, tilePositions, tiles.FirstOrDefault(), null);

        }
    }
}
