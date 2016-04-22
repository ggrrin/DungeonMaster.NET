using DungeonMasterEngine.Helpers;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public class LegacyMapBuilder : BuilderBase
    {
        private readonly Dictionary<int, DungeonLevel> loadedLevels = new Dictionary<int, DungeonLevel>();
        private List<Tile> outputTiles;
        private Stack<TileInfo<TileData>> stack;
        private Point start;
        private BitMapMemory bitMap;
        private TileInfo<TileData> currentTile;
        private LegacyTileCreator legacyTileCreator;

        private Texture2D[] doorTextures;
        private Texture2D[] wallTextures;
        private Texture2D[] floorTextures;

        public DungeonData Data { get; }

        public DungeonMap CurrentMap { get; private set; }

        public Dictionary<Point, Tile> TilesPositions { get; private set; }

        public LegacyItemCreator ItemCreator { get; private set; }
        public WallActuatorCreator WallActuatorCreator { get; private set; }

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

        private void Initialize(int level, Point? startTile)
        {
            CurrentLevel = level;
            CurrentMap = Data.Maps[level];
            ItemCreator = new LegacyItemCreator(this);
            legacyTileCreator = new LegacyTileCreator(this);
            WallActuatorCreator = new WallActuatorCreator(this);
            InitializeMapTextures();

            outputTiles = new List<Tile>();
            stack = new Stack<TileInfo<TileData>>();
            start = startTile ?? new Point(Data.StartPosition.Position.X, Data.StartPosition.Position.Y);
            TilesPositions = new Dictionary<Point, Tile>();
            bitMap = new BitMapMemory(CurrentMap.OffsetX, CurrentMap.OffsetY, CurrentMap.Width, CurrentMap.Height);
        }

        public override DungeonLevel GetLevel(int level, Dungeon dungeon, Point? startTile)
        {
            DungeonLevel dungeonLevel = null;
            if (Data.Maps.Count <= level)
                throw new ArgumentException("Level does not exist.");
            if (loadedLevels.TryGetValue(level, out dungeonLevel))
                return dungeonLevel;

            Initialize(level, startTile);
            stack.Push(new TileInfo<TileData>
            {
                Position = start,
                Tile = CurrentMap[start.X, start.Y]
            });
            bitMap[start.X, start.Y] = true;

            while (stack.Count > 0)
            {
                currentTile = stack.Pop();
                var nextTiles = CreateTile(currentTile).Where(t => t.Tile != null && t.Tile.GetType() != typeof (WallTileData) && !bitMap[t.Position.X, t.Position.Y]);

                foreach (var t in nextTiles) //recursion
                {
                    stack.Push(t);
                    bitMap[t.Position.X, t.Position.Y] = true;
                }
            }

            SetupNeighbours(TilesPositions, outputTiles);
            SetupItems();

            dungeonLevel = new DungeonLevel(dungeon, outputTiles, level, TilesPositions, TilesPositions[start], legacyTileCreator.MiniMap);
            loadedLevels.Add(level, dungeonLevel);
            return dungeonLevel;
        }

        private IEnumerable<TileInfo<TileData>> CreateTile(TileInfo<TileData> currentTile)
        {
            var newTile = legacyTileCreator.GetTile(currentTile);
            TilesPositions.Add(currentTile.Position, newTile); //remember Tiles-position association
            outputTiles.Add(newTile); //remember created tile

            return GetNeigbourTiles(currentTile.Position, CurrentMap).Concat(legacyTileCreator.Successors); //add nextTiles advised by CurrentTile
        }

        public IEnumerable<TileInfo<TileData>> GetNeigbourTiles(Point position, DungeonMap map)
        {
            Point[] positions = {position + new Point
            {
                Y = -1
            }, //North
                position + new Point
                {
                    X = 1
                }, //East
                position + new Point
                {
                    Y = 1
                }, //South
                position + new Point
                {
                    X = -1
                } //West
            };

            return positions.Select(p => new TileInfo<TileData>
            {
                Position = p,
                Tile = map[p.X, p.Y]
            });
        }

        private void SetupItems()
        {
            foreach (var tile in outputTiles)
            {
                WallActuatorCreator.CreateSetupActuators(tile);
                //TODO
                SetupFloorItems(tile);
            }
        }

        //private void SetupWallItems(Tile tile)
        //{
        //    Func<ItemData, Point, bool> isValid = (o, p) => !o.Processed && o.TilePosition == (new Point(-1) * p).ToDirection();
        //    foreach (var neighbour in tile.Neighbours.Where(p => p.Key == null))
        //    {
        //        var wall = CurrentMap.GetTileData(tile.GridPosition + neighbour.Value) as WallTileData;
        //        if (wall != null && wall.HasItemsList)
        //        {
        //            var validActuators = wall.Actuators.Where(x => isValid(x, neighbour.Value) && x.AcutorType != 5 && x.AcutorType != 6); //do not evaluate And/Or gate, counters
        //            foreach (ActuatorItemData o in validActuators)
        //                LegacyItemCreator.AddWallItem(o, tile, wall);

        //            foreach (var t in wall.TextTags.Where(x => isValid(x, neighbour.Value)))
        //                LegacyItemCreator.AddWallItem(t, tile, wall);
        //        }
        //    }
        //}

        private void SetupFloorItems(Tile tile)
        {
            //TODO 24 22 target tile null
            var tileData = CurrentMap[tile.GridPosition.X, tile.GridPosition.Y];

            foreach (var actuator in tileData.Actuators.Where(act => !act.Processed))
                ItemCreator.AddFloorItem(actuator, tile);

            foreach (var item in tileData.GrabableItems.Where(i => !i.Processed))
                ItemCreator.AddFloorItem(item, tile);

            //foreach (var creatre in tileData.Creatures.Where(i => !i.Processed))
            //    ItemCreator.AddFloorItem(creatre, tile);
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




        public Tile GetTargetTile(ActuatorItemData callingActuator)
        {
            var targetPos = ((RmtTrg) callingActuator.ActLoc).Position.Position.ToAbsolutePosition(CurrentMap);

            Tile targetTile = null;
            if (TilesPositions.TryGetValue(targetPos, out targetTile))
                return targetTile;
            else
            {
                //try find tile in raw data, and than actuator, add it to Tiles Positions
                var virtualTileData = CurrentMap[targetPos.X, targetPos.Y];
                if (virtualTileData.Actuators.Count > 0) //virtual tile will be proccessed at the and so any checking shouldnt be necessary
                {
                    var newTile = new LogicTile(targetPos.ToGridVector3(CurrentLevel));
                    newTile.Gates = virtualTileData.Actuators.Where(x => x.ActuatorType == 5).Select(y => InitLogicGates(y, newTile)).ToArray(); //recurse
                    newTile.Counters = virtualTileData.Actuators.Where(x => x.ActuatorType == 6).Select(y => InitCounters(y, newTile)).ToArray(); //recurse

                    TilesPositions.Add(targetPos, targetTile = newTile); //subitems will be processed 
                }

                return targetTile; //TODO (if null)  think out what to do 
                //Acutor at the begining references wall near by with tag only ... what to do ? 
            }
        }

        private CounterActuator InitCounters(ActuatorItemData gateActuator, Tile gateActuatorTile)
        {
            //if nextTarget tile is current tile do not call recurese
            Tile nextTargetTile = gateActuatorTile.GridPosition == ((RmtTrg) gateActuator.ActLoc).Position.Position.ToAbsolutePosition(CurrentMap) ? gateActuatorTile : GetTargetTile(gateActuator);

            return new CounterActuator(nextTargetTile, new ActionStateX((ActionState) gateActuator.Action, ((RmtTrg) gateActuator.ActLoc).Direction), gateActuator.Data, gateActuatorTile.Position);
        }

        private LogicGate InitLogicGates(ActuatorItemData gateActuator, Tile gateActuatorTile)
        {
            //if nextTarget tile is current tile do not call recurese
            Tile nextTargetTile = gateActuatorTile.GridPosition == ((RmtTrg) gateActuator.ActLoc).Position.Position.ToAbsolutePosition(CurrentMap) ? gateActuatorTile : GetTargetTile(gateActuator);

            return new LogicGate(nextTargetTile, new ActionStateX((ActionState) gateActuator.Action, ((RmtTrg) gateActuator.ActLoc).Direction), gateActuatorTile.Position, (gateActuator.Data & 0x10) == 0x10, (gateActuator.Data & 0x20) == 0x20, (gateActuator.Data & 0x40) == 0x40, (gateActuator.Data & 0x80) == 0x80);
        }

        public Vector3 GetFloorPosition(TilePosition tilePosition, Tile currentTile)
        {
            Vector3 offset = Vector3.Zero;
            const float scalarOffset = 0.25f;

                switch (tilePosition)
                {
                    case TilePosition.North_TopLeft:
                        offset = new Vector3(scalarOffset, 0, scalarOffset);
                        break;
                    case TilePosition.East_TopRight:
                        offset = new Vector3(1 - scalarOffset, 0, scalarOffset);
                        break;
                    case TilePosition.South_BottomLeft:
                        offset = new Vector3(scalarOffset, 0, 1 - scalarOffset);
                        break;
                    case TilePosition.West_BottomRight:
                        offset = new Vector3(1 - scalarOffset, 0, 1 - scalarOffset);
                        break;
                }

            return currentTile.Position + offset;
        }

        public Vector3 GetWallPosition(TilePosition tilePosition, Tile currentTile)
        {
            Vector3 offset = Vector3.Zero;
            const float scalarOffset = 0.25f;
            switch (tilePosition)
            {
                case TilePosition.North_TopLeft:
                    offset = new Vector3(scalarOffset, scalarOffset, 1 - scalarOffset);
                    break;
                case TilePosition.East_TopRight:
                    offset = new Vector3(0, scalarOffset, 1 - scalarOffset);
                    break;
                case TilePosition.South_BottomLeft:
                    offset = new Vector3(scalarOffset, scalarOffset, 0);
                    break;
                case TilePosition.West_BottomRight:
                    offset = new Vector3(1 - scalarOffset, scalarOffset, scalarOffset);
                    break;
            }

            return currentTile.Position + offset;
        }

        public bool PrepareActuatorData(ActuatorItemData i, out Tile targetTile, out IConstrain constrain, out Texture2D decoration, bool putOnWall)
        {
            targetTile = GetTargetTile(i);
            constrain = null;
            decoration = null;

            if (i.Data > 0)
                constrain = new GrabableItemConstrain(i.Data, i.IsRevertable);
            else
                constrain = new NoConstrain();

            if (i.IsLocal)
                throw new NotSupportedException("yet");

            if (i.Decoration > 0)
                decoration = putOnWall ? WallTextures[i.Decoration - 1] : FloorTextures[i.Decoration - 1];

            return true;
        }
    }

    public struct TileInfo<T> where T : TileData
    {
        public T Tile { get; set; }

        public Point Position { get; set; }
    }
}
