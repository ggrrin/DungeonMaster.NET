using DungeonMasterEngine.Helpers;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterParser.Items;
using DungeonMasterParser.Items.@abstract;
using DungeonMasterParser.Tiles;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public partial class OldDungeonBuilder : IDungonBuilder
    {
        private DungeonData data;

        private Dictionary<int, DungeonLevel> loadedLevels = new Dictionary<int, DungeonLevel>();

        private ItemCreator itemCreator;

        public OldDungeonBuilder()
        {
            var p = new DungeonParser();
            p.Parse();
            data = p.Data;

            itemCreator = new ItemCreator(this);
        }

        private DungeonMap map;
        private List<Tile> outputTiles;
        private Stack<TileInfo<DungeonMasterParser.Tiles.Tile>> stack;
        private Point start;
        private Dictionary<Point, Tile> tilesPositions;
        private BitMapMemory bitMap;
        private TileCreator tileCreator;
        private TileInfo<DungeonMasterParser.Tiles.Tile> currentTile;
        private Dungeon dungeon;


        public DungeonLevel GetLevel(int level, Dungeon d, Point? startTile)
        {
            dungeon = d;

            if (data.Maps.Count <= level)
                throw new ArgumentException("Level does not exists");

            DungeonLevel dungeonLevel = null;
            if (loadedLevels.TryGetValue(level, out dungeonLevel))
                return dungeonLevel;

            map = data.Maps[level];
            InitializeMapTextures();
            outputTiles = new List<Tile>();
            stack = new Stack<TileInfo<DungeonMasterParser.Tiles.Tile>>();
            start = startTile ?? new Point(data.StartPosition.Position.X, data.StartPosition.Position.Y);
            tilesPositions = new Dictionary<Point, Tile>();
            bitMap = new BitMapMemory(map.OffsetX, map.OffsetY, map.Width, map.Height);
            tileCreator = new TileCreator(this, level);


            //initialize
            stack.Push(new TileInfo<DungeonMasterParser.Tiles.Tile> { Position = start, Tile = map[start.X, start.Y] });
            bitMap[start.X, start.Y] = true;

            while (stack.Count > 0)
            {
                currentTile = stack.Pop();
                var nextTiles = CreateTile(currentTile);

                foreach (var t in nextTiles)//recursion
                    if (t.Tile != null && t.Tile.GetType() != typeof(WallTile) && !bitMap[t.Position.X, t.Position.Y])
                    {
                        stack.Push(t);
                        bitMap[t.Position.X, t.Position.Y] = true;
                    }
            }

            SetupNeighbours(tilesPositions, outputTiles);
            SetupItems();

            dungeonLevel = new DungeonLevel(d, outputTiles, level, tilesPositions, tilesPositions[start], tileCreator.MiniMap);
            loadedLevels.Add(level, dungeonLevel);
            return dungeonLevel;
        }

        private Texture2D[] doorTextures;
        private Texture2D[] wallTextures;
        private Texture2D[] floorTextures;
        private Texture2D defaultDoorTexture;
        private Texture2D defaultMapDoorTypeTexture;
        private void InitializeMapTextures()
        {
            if (map.DoorType0Index == 0)
                defaultDoorTexture = ResourceProvider.Instance.Content.Load<Texture2D>("Textures/DefaultDoor");
            else
                throw new NotSupportedException();

            defaultMapDoorTypeTexture = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{Enum.GetName(map.DoorType.GetType(), map.DoorType)}");

            doorTextures = new Texture2D[map.DoorDecorationCount];
            for (int i = 0; i < doorTextures.Length; i++)
                doorTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{map.DoorDecorations[i]}");

            wallTextures = new Texture2D[map.WallGraphicsCount];
            for (int i = 0; i < wallTextures.Length; i++)
                wallTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{map.WallDecorations[i]}");

            floorTextures = new Texture2D[map.FloorGraphicsCount];
            for (int i = 0; i < floorTextures.Length; i++)
                floorTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{map.FloorDecorations[i]}");
        }

        private List<TileInfo<DungeonMasterParser.Tiles.Tile>> CreateTile(TileInfo<DungeonMasterParser.Tiles.Tile> currentTile)
        {
            var nextTiles = GetNeigbourTiles(currentTile.Position, map);
            var newTile = tileCreator.GetTile(currentTile);//, GetWallFaces(nextTiles));
            nextTiles.AddRange(tileCreator.Successors);//add nextTiles advised by currentTile
            tilesPositions.Add(currentTile.Position, newTile);//remember Tiles-position association
            outputTiles.Add(newTile);

            return nextTiles;
        }

        private List<TileInfo<DungeonMasterParser.Tiles.Tile>> GetNeigbourTiles(Point position, DungeonMap map)
        {
            var l = new List<TileInfo<DungeonMasterParser.Tiles.Tile>>();

            Point[] positions = new Point[]
            {
                position + new Point {Y= -1 }, //North
                position + new Point {X= 1 }, //East
                position + new Point {Y= 1 }, //South
                position + new Point {X= -1 } //West
            };

            foreach (var p in positions)
                l.Add(new TileInfo<DungeonMasterParser.Tiles.Tile> { Position = p, Tile = map[p.X, p.Y] });

            return l;
        }

        public static void SetupNeighbours(IDictionary<Point, Tile> tilesPositions, IList<Tile> tiles)
        {
            foreach (var t in tiles)
            {
                Tile north = null;
                Tile east = null;
                Tile south = null;
                Tile west = null;

                var neighbours = new Neighbours();
                if (tilesPositions.TryGetValue(t.GridPosition + new Point(0, -1), out north))
                    neighbours.North = north;

                if (tilesPositions.TryGetValue(t.GridPosition + new Point(1, 0), out east))
                    neighbours.East = east;

                if (tilesPositions.TryGetValue(t.GridPosition + new Point(0, 1), out south))
                    neighbours.South = south;

                if (tilesPositions.TryGetValue(t.GridPosition + new Point(-1, 0), out west))
                    neighbours.West = west;
                t.Neighbours = neighbours;
            }
        }


        private void SetupItems()
        {
            foreach (var tile in outputTiles)
            {
                SetupWallItems(tile);
                SetupFloorItems(tile);
            }
        }

        private void SetupFloorItems(Tile tile)
        {
            //TODO 24 22 target tile null
            var tileData = map[tile.GridPosition.X, tile.GridPosition.Y];

            foreach (var act in tileData.Actuators)
                if (!act.Processed)
                    itemCreator.AddFloorItem(act, tile);

            foreach (var i in tileData.Items)
                if (!i.Processed)
                    itemCreator.AddFloorItem(i, tile);
        }

        private void SetupWallItems(Tile tile)
        {
            Func<SuperItem, Point, bool> isValid = (o, p) => !o.Processed && o.TilePosition == (new Point(-1) * p).ToDirection();

            foreach (var neighbour in tile.Neighbours.Where((p) => p.Key == null))
            {
                var pos = tile.GridPosition + neighbour.Value;
                var wall = map[pos.X, pos.Y] as WallTile;
                if (wall != null && wall.HasItemsList)
                {
                    foreach (ActuatorItem o in wall.Actuators.ReverseLazy().Where(x => !(x.AcutorType == 5 || x.AcutorType == 6) /*do not evaluate And/Or gate*/))//last actuator in the list is active
                        if (isValid(o, neighbour.Value))
                            itemCreator.AddWallItem(o, tile, wall);

                    var tags = from item in wall.Items where item.GetType() == typeof(TextDataItem) select (TextDataItem)item;
                    foreach (var t in tags)
                        if (isValid(t, neighbour.Value))
                            itemCreator.AddWallItem(t, tile, wall);
                    //TODO process rest => items should be processed by acutors builder themselves, nevertheless check is necessary
                }
            }
        }

        struct TileInfo<T> where T : DungeonMasterParser.Tiles.Tile
        {
            public T Tile { get; set; }

            public Point Position { get; set; }

        }
    }
}
