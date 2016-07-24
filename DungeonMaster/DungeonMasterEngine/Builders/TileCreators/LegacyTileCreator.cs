using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.Builders.ActuatorCreators;
using DungeonMasterEngine.Builders.CreatureCreators;
using DungeonMasterEngine.Builders.ItemCreators;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrabableItem = DungeonMasterEngine.DungeonContent.GrabableItems.GrabableItem;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders.TileCreators
{
    public interface ILegacyTileCreator : ICreator
    {
        Texture2D MiniMap { get; }

        Tile GetTile(TileInfo<TileData> tileInfo);
    }

    public class LegacyTileCreator : ILegacyTileCreator, ITileCreator<Tile>
    {
        private Texture2D texture;
        private Color[] miniMapData;

        protected readonly LegacyMapBuilder builder;
        private readonly ILogicActuatorCreator logicActuatorCreator;
        private readonly ICreatureCreator creatureCreator;
        private readonly ISidesCreator sidesCreator;

        protected Vector3 tilePosition;
        protected Point currentGridPosition;
        protected TileInitializer initializer;


        public Texture2D MiniMap
        {
            get
            {
                texture.SetData(miniMapData);
                return texture;
            }
        }

        public LegacyTileCreator(LegacyMapBuilder builder, ISidesCreator sidesCreator, ILogicActuatorCreator logicActuatorCreator, ICreatureCreator creatureCreator)
        {
            this.builder = builder;
            this.sidesCreator = sidesCreator;
            this.logicActuatorCreator = logicActuatorCreator;
            this.creatureCreator = creatureCreator;
        }

        private void SetMinimapTile(Color color) => miniMapData[(int)tilePosition.Z * texture.Width + (int)tilePosition.X] = color;

        public void Reset()
        {
            tilePosition = default(Vector3);
            currentGridPosition = default(Point);
            initializer = null;

            texture = new Texture2D(ResourceProvider.Instance.Device, this.builder.CurrentMap.OffsetX + this.builder.CurrentMap.Width, this.builder.CurrentMap.OffsetY + this.builder.CurrentMap.Height);
            miniMapData = new Color[texture.Width * texture.Height];

        }

        public virtual Tile GetTile(TileInfo<TileData> tileInfo)
        {
            tilePosition = new Vector3(tileInfo.Position.X, -builder.CurrentLevelIndex, tileInfo.Position.Y);
            currentGridPosition = tileInfo.Position;
            var tile = tileInfo.Tile.GetTile(this);

            if (initializer != null)
            {
                foreach (var creatureData in tileInfo.Tile.Creatures)
                    creatureCreator.AddCreatureToTile(creatureData, tile, tileInfo.Position);

                initializer.GridPosition = currentGridPosition;
                builder.TileInitializers.Add(initializer);
                initializer = null;
            }
            return tile;
        }

        public Tile GetTile(FloorTileData t)
        {
            SetMinimapTile(Color.White);

            var initalizer = new FloorInitializer();
            var res = new FloorTile(initalizer);
            sidesCreator.SetupSidesAsync(initalizer, currentGridPosition, res);
            res.Renderer = builder.Factories.RenderersSource.GetTileRenderer(res);
            this.initializer = initalizer;
            return res;
        }

        public Tile GetTile(DoorTileData t)
        {
            if (t.Door == null)
                throw new ArgumentNullException("Invalid map format. Door item should be at door tile.");

            SetMinimapTile(Color.Purple);

            var initializer = new DoorInitializer { Door = CreateDoor(t) };
            initializer.HasButton = t.Door.HasButton;


            initializer.Direction = t.Orientation == Orientation.NorthSouth ? MapDirection.North : MapDirection.East;

            var res = new DoorTile(initializer);
            sidesCreator.SetupSidesAsync(initializer, currentGridPosition, res);
            res.Renderer = builder.Factories.RenderersSource.GetDoorTileRenderer(res, builder.WallTexture, builder.DoorButtonTexture);
            this.initializer = initializer;
            return res;
        }

        public Door CreateDoor(DoorTileData tile)
        {
            var doorInfo = builder.GetCurrentDoor(tile.Door.DoorType);
            var res = new Door(tile.State == DoorState.Open, doorInfo.Resistance, tile.Door.IsChoppingDestructible, tile.Door.IsFireballDestructible, doorInfo.ItemsPassThrough, doorInfo.CreatureSeeThrough);
            res.Renderer = builder.Factories.RenderersSource.GetDoorRenderer(tile.Door.OrnamentationID == 0 ? builder.DefaultDoorTexture : builder.DoorTextures[tile.Door.OrnamentationID - 1]);
            return res;
        }

        public Tile GetTile(PitTileData t)
        {
            SetMinimapTile(Color.Orange);

            var pitInitializer = new PitInitializer
            {
                Imaginary = t.Imaginary,
                Invisible = t.Invisible,
                IsOpen = t.IsOpen
            };

            var res = new Pit(pitInitializer);
            sidesCreator.SetupSidesAsync(pitInitializer, currentGridPosition, res);
            res.Renderer = builder.Factories.RenderersSource.GetPitTileRenderer(res);
            initializer = pitInitializer;
            return res;
        }

        public Tile GetTile(TeleporterTileData t)
        {
            if (t.Teleport == null)
                throw new InvalidOperationException("Invalid map format. Teleport tile has to have teleport");

            SetMinimapTile(Color.Blue);

            var initializer = new TeleprotInitializer
            {
                Direction = t.Teleport.Rotation.ToMapDirection(),
                AbsoluteDirection = t.Teleport.RotationType == RotationType.Absolute,
                Open = t.IsOpen,
                ScopeConstrain = GetTeleportScopeType(t.Teleport.Scope),
                TargetTilePosition = t.Teleport.DestinationPosition.ToAbsolutePosition(builder.Data.Maps[t.Teleport.MapIndex]),
                NextLevelIndex = t.Teleport.MapIndex,
                Visible = t.IsVisible,
            };


            var res = new TeleportTile(initializer);
            SetupTeleportSides(initializer, currentGridPosition, true, res);
            res.Renderer = builder.Factories.RenderersSource.GetTileRenderer(res);
            this.initializer = initializer;
            return res;
        }

        private async void SetupTeleportSides(TeleprotInitializer initializer, Point gridPosition, bool randomDecoration, ITile tile)
        {
            await sidesCreator.SetupSidesAwaitableAsync(initializer, currentGridPosition, tile);
            initializer.FloorSide.Renderer = builder.Factories.RenderersSource.GetTeleportFloorSideRenderer(initializer.FloorSide, builder.WallTexture, builder.TeleportTexture);
        }

        public Tile GetTile(WallTileData t)
        {
            var logicSensors = t.Actuators
                .Where(x => x.ActuatorType == 5 || x.ActuatorType == 6)
                .ToArray();

            if (logicSensors.Any())
            {
                var logicTileInitializer = new LogicTileInitializer();
                initializer = logicTileInitializer;
                logicActuatorCreator.SetLogicActuator(logicTileInitializer, logicSensors);
                return new LogicTile(logicTileInitializer);
            }
            else
            {
                return null;
            }
        }


        public Tile GetTile(TrickTileData t)
        {
            SetMinimapTile(Color.Green);

            var trickTileInitializer = new WallIlusionInitializer
            {
                Imaginary = t.IsImaginary,
                Open = t.IsOpen,
                RandomDecoration = t.RandomDecoration,
            };

            var res = new WallIlusion(trickTileInitializer);
            SetupWallIllusionSidesAsync(trickTileInitializer, t.RandomDecoration, res);
            res.Renderer = builder.Factories.RenderersSource.GetWallIllusionTileRenderer(res, builder.WallTexture);

            initializer = trickTileInitializer;
            return res;
        }

        private async void SetupWallIllusionSidesAsync(WallIlusionInitializer initializer, int? randomDecoration, ITile tile)
        {
            await sidesCreator.SetupSidesAwaitableAsync(initializer, currentGridPosition, tile);

            var trickSides = MapDirection.Sides.Except(initializer.WallSides.Select(x => x.Face))
                .Select(x =>
                {
                    var decoration = randomDecoration != null ? builder.WallTextures[randomDecoration.Value] : null;
                    var res = new TileSide(x);
                    res.Renderer = builder.Factories.RenderersSource.GetWallIllusionTileSideRenderer(res, builder.WallTexture, decoration);
                    return res;
                });

            initializer.WallSides = initializer.WallSides
                .Concat(trickSides)
                .ToArray();
        }

        public Tile GetTile(StairsTileData t)
        {
            SetMinimapTile(Color.Yellow);
            StairsInitializer stairsInitializer = new StairsInitializer { Down = t.Direction == VerticalDirection.Down };

            var res = new Stairs(stairsInitializer);
            sidesCreator.SetupSidesAsync(stairsInitializer, currentGridPosition, res);
            if (t.Direction == VerticalDirection.Down)
            {
                var upperStairsEntry = FindStairsEntryDirection(tilePosition.ToGrid(), -(int) tilePosition.Y);
                var lowerStairsEntry = FindStairsEntryDirection(tilePosition.ToGrid(), -(int) tilePosition.Y + 1);
                res.Renderer = builder.Factories.RenderersSource.GetUpperStairsTileRenderer(upperStairsEntry, lowerStairsEntry, res, builder.WallTexture);
            }
            else
            {
                res.Renderer = builder.Factories.RenderersSource.GetLowerStairsTileRenderer(res, builder.WallTexture);
            }

            this.initializer = stairsInitializer;
            return res;
        }

        private MapDirection FindStairsEntryDirection(Point pos, int level)
        {
            var map = builder.Data.Maps[level];
            return MapDirection.Sides.First(s => !(map.GetTileData(pos + s.RelativeShift) is WallTileData));
        }

        private IConstrain GetTeleportScopeType(TeleportScope scope)
        {
            switch (scope)
            {
                case TeleportScope.Creatures:
                    return new TypeConstrain(typeof(Creature));
                case TeleportScope.Everything:
                    return new NoConstrain();
                case TeleportScope.Items:
                    return new TypeConstrain(typeof(GrabableItem));
                case TeleportScope.ItemsOrParty:
                    return new OrConstrain(new List<IConstrain>
                    {
                        new TypeConstrain(typeof(GrabableItem)),
                        new PartyConstrain()
                    });
                default:
                    throw new InvalidOperationException();
            }
        }

    }
}
