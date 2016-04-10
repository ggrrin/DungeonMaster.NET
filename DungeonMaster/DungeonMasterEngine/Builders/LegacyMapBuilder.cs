using DungeonMasterEngine.Helpers;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterParser.Items;
using DungeonMasterParser.Tiles;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public class LegacyMapBuilder : BuilderBase
    {
        private readonly Dictionary<int, DungeonLevel> loadedLevels = new Dictionary<int, DungeonLevel>();
        private List<Tile> outputTiles;
        private Stack<TileInfo<DungeonMasterParser.Tiles.TileData>> stack;
        private Point start;
        private BitMapMemory bitMap;

        private Texture2D[] doorTextures;
        private Texture2D[] wallTextures;
        private Texture2D[] floorTextures;

        public DungeonData Data { get; }
        public DungeonMap CurrentMap { get; private set; }
        public Dictionary<Point, Tile> TilesPositions { get; private set; }
        public TileInfo<DungeonMasterParser.Tiles.TileData> CurrentTile { get; private set; }
        public LegacyItemCreator LegacyItemCreator { get; private set; }
        public LegacyTileCreator LegacyTileCreator { get; private set; }

        public Texture2D defaultDoorTexture { get; private set; }
        public Texture2D defaultMapDoorTypeTexture { get; private set; }
        public IReadOnlyList<Texture2D> DoorTextures => doorTextures;
        public IReadOnlyList<Texture2D> WallTextures => wallTextures;
        public IReadOnlyList<Texture2D> FloorTextures => floorTextures;
        public int CurrentLevel { get; private set; }

        public LegacyMapBuilder()
        {
            var p = new DungeonParser();
            p.Parse();
            Data = p.Data;
        }

        public override DungeonLevel GetLevel(int level, Dungeon dungeon, Point? startTile)
        {
            DungeonLevel dungeonLevel = null;
            if (Data.Maps.Count <= level)
                throw new ArgumentException("Level does not exist.");
            if (loadedLevels.TryGetValue(level, out dungeonLevel))
                return dungeonLevel;

            Initialize(level, startTile);
            stack.Push(new TileInfo<DungeonMasterParser.Tiles.TileData>
            {
                Position = start,
                Tile = CurrentMap[start.X, start.Y]
            });
            bitMap[start.X, start.Y] = true;

            while (stack.Count > 0)
            {
                CurrentTile = stack.Pop();
                var nextTiles = CreateTile(CurrentTile);

                foreach (var t in nextTiles) //recursion
                    if (t.Tile != null && t.Tile.GetType() != typeof(WallTileData) && !bitMap[t.Position.X, t.Position.Y])
                    {
                        stack.Push(t);
                        bitMap[t.Position.X, t.Position.Y] = true;
                    }
            }

            SetupNeighbours(TilesPositions, outputTiles);
            SetupItems();

            dungeonLevel = new DungeonLevel(dungeon, outputTiles, level, TilesPositions, TilesPositions[start], LegacyTileCreator.MiniMap);
            loadedLevels.Add(level, dungeonLevel);
            return dungeonLevel;
        }

        private void Initialize(int level, Point? startTile)
        {
            CurrentLevel = level;
            CurrentMap = Data.Maps[level];
            LegacyItemCreator = new LegacyItemCreator(this);
            LegacyTileCreator = new LegacyTileCreator(this);
            InitializeMapTextures();

            outputTiles = new List<Tile>();
            stack = new Stack<TileInfo<DungeonMasterParser.Tiles.TileData>>();
            start = startTile ?? new Point(Data.StartPosition.Position.X, Data.StartPosition.Position.Y);
            TilesPositions = new Dictionary<Point, Tile>();
            bitMap = new BitMapMemory(CurrentMap.OffsetX, CurrentMap.OffsetY, CurrentMap.Width, CurrentMap.Height);
        }

        private IEnumerable<TileInfo<DungeonMasterParser.Tiles.TileData>> CreateTile(TileInfo<DungeonMasterParser.Tiles.TileData> currentTile)
        {
            var newTile = LegacyTileCreator.GetTile(currentTile); 
            TilesPositions.Add(currentTile.Position, newTile); //remember Tiles-position association
            outputTiles.Add(newTile); //remember created tile

            return GetNeigbourTiles(currentTile.Position, CurrentMap)
                .Concat(LegacyTileCreator.Successors);//add nextTiles advised by CurrentTile
        }

        public IEnumerable<TileInfo<DungeonMasterParser.Tiles.TileData>> GetNeigbourTiles(Point position, DungeonMap map)
        {
            Point[] positions = {
                position + new Point { Y = -1 }, //North
                position + new Point { X = 1 }, //East
                position + new Point { Y = 1 }, //South
                position + new Point { X = -1 } //West
            };

            return positions.Select(p => new TileInfo<DungeonMasterParser.Tiles.TileData>
            {
                Position = p,
                Tile = map[p.X, p.Y]
            });
        }

        private void SetupItems()
        {
            foreach (var tile in outputTiles)
            {

                SetupFloorItems(tile);
            }
        }

        private void SetupWallItems(Tile tile)
        {
            Func<ItemData, Point, bool> isValid = (o, p) => !o.Processed && o.TilePosition == (new Point(-1) * p).ToDirection();
            foreach (var neighbour in tile.Neighbours.Where(p => p.Key == null))
            {
                var wall = CurrentMap.GetTileData(tile.GridPosition + neighbour.Value) as WallTileData;
                if (wall != null && wall.HasItemsList)
                {
                    var validActuators = wall.Actuators.Where(x => isValid(x, neighbour.Value) && x.AcutorType != 5 && x.AcutorType != 6); //do not evaluate And/Or gate, counters
                    foreach (ActuatorItemData o in validActuators)
                        LegacyItemCreator.AddWallItem(o, tile, wall);

                    foreach (var t in wall.TextTags.Where(x => isValid(x, neighbour.Value)))
                        LegacyItemCreator.AddWallItem(t, tile, wall);
                }
            }
        }

        private void SetupFloorItems(Tile tile)
        {
            //TODO 24 22 target tile null
            var tileData = CurrentMap[tile.GridPosition.X, tile.GridPosition.Y];

            foreach (var actuator in tileData.Actuators.Where(act => !act.Processed))
                LegacyItemCreator.AddFloorItem(actuator, tile);

            foreach (var item in tileData.GrabableItems.Where(i => !i.Processed))
                LegacyItemCreator.AddFloorItem(item, tile);

            foreach (var creatre in tileData.Creatures.Where(i => !i.Processed))
                LegacyItemCreator.AddFloorItem(creatre, tile);
        }

        private void InitializeMapTextures()
        {
            if (CurrentMap.DoorType0Index == 0)
                defaultDoorTexture = ResourceProvider.Instance.Content.Load<Texture2D>("Textures/DefaultDoor");
            else
                throw new NotSupportedException();

            defaultMapDoorTypeTexture = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{Enum.GetName(CurrentMap.DoorType.GetType(), CurrentMap.DoorType)}");

            doorTextures = new Texture2D[CurrentMap.DoorDecorationCount];
            for (int i = 0; i < doorTextures.Length; i++)
                doorTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.DoorDecorations[i]}");

            wallTextures = new Texture2D[CurrentMap.WallGraphicsCount];
            for (int i = 0; i < wallTextures.Length; i++)
                wallTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.WallDecorations[i]}");

            floorTextures = new Texture2D[CurrentMap.FloorGraphicsCount];
            for (int i = 0; i < floorTextures.Length; i++)
                floorTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.FloorDecorations[i]}");
        }

    }

    public struct TileInfo<T> where T : TileData
    {
        public T Tile { get; set; }

        public Point Position { get; set; }
    }
}
