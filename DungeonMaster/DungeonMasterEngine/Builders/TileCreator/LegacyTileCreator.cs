using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.Builders.ActuatorCreator;
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

namespace DungeonMasterEngine.Builders.TileCreator
{
    public class LegacyTileCreator : ITileCreator<Tile>
    {
        private readonly LegacyMapBuilder builder;
        private readonly Color[] miniMapData;
        private readonly Texture2D texture;
        private readonly LogicActuatorCreator logicActuatorCreator;
        private readonly CreatureCreator.CreatureCreator creatureCreator;
        private Vector3 tilePosition;

        public int level => builder.CurrentLevel;

        public Texture2D MiniMap
        {
            get
            {
                texture.SetData(miniMapData);
                return texture;
            }
        }


        public LegacyTileCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
            texture = new Texture2D(ResourceProvider.Instance.Device, this.builder.CurrentMap.OffsetX + this.builder.CurrentMap.Width, this.builder.CurrentMap.OffsetY + this.builder.CurrentMap.Height);
            miniMapData = new Color[texture.Width * texture.Height];
            sidesCreator = new SidesCreator(builder);
            logicActuatorCreator = new LogicActuatorCreator(builder);
            creatureCreator = new CreatureCreator.CreatureCreator(builder);
        }

        private void SetMinimapTile(Color color) => miniMapData[(int)tilePosition.Z * texture.Width + (int)tilePosition.X] = color;

        public virtual  Tile GetTile(TileInfo<TileData> tileInfo)
        {
            tilePosition = new Vector3(tileInfo.Position.X, -level, tileInfo.Position.Y);
            GridPosition = tileInfo.Position;
            var tile = tileInfo.Tile.GetTile(this);

            if (initializer != null)
            {
                initializer.Creatures = tileInfo.Tile.Creatures.SelectMany(x => creatureCreator.AddCreature(x, tile)).ToArray();
                builder.Creatures.AddRange(initializer.Creatures);
                initializer.GridPosition = GridPosition;
                builder.TileInitializers.Add(initializer);
                initializer = null;
            }
            return tile;
        }

        public Point GridPosition { get; private set; }

        private readonly SidesCreator sidesCreator;
        private TileInitializer initializer;

        public Tile GetTile(FloorTileData t)
        {
            SetMinimapTile(Color.White);

            var initalizer = new FloorInitializer();
            var res = new FloorTile(initalizer);
            sidesCreator.SetupSidesAsync(initalizer, GridPosition, t.AllowRandomDecoration, res);
            res.Renderer = builder.RendererSource.GetTileRenderer(res);
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
            sidesCreator.SetupSidesAsync(initializer, GridPosition, false, res);
            res.Renderer = builder.RendererSource.GetDoorTileRenderer(res, builder.WallTexture, builder.DoorButtonTexture);
            this.initializer = initializer;
            return res;
        }

        public Door CreateDoor(DoorTileData tile)
        {
            var doorInfo = builder.GetCurrentDoor(tile.Door.DoorType);
            var res = new Door(tile.State == DoorState.Open, doorInfo.Resistance, tile.Door.IsChoppingDestructible, tile.Door.IsFireballDestructible, doorInfo.ItemsPassThrough, doorInfo.CreatureSeeThrough);
            res.Renderer = builder.RendererSource.GetDoorRenderer(tile.Door.OrnamentationID == 0 ? builder.defaultDoorTexture : builder.DoorTextures[tile.Door.OrnamentationID - 1]);
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
            sidesCreator.SetupSidesAsync(pitInitializer, GridPosition, false, res);
            res.Renderer = builder.RendererSource.GetPitTileRenderer(res);
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
            SetupTeleportSides(initializer, GridPosition, true, res);
            res.Renderer = builder.RendererSource.GetTileRenderer(res);
            this.initializer = initializer;
            return res;
        }

        private async void SetupTeleportSides(TeleprotInitializer initializer, Point gridPosition, bool randomDecoration, ITile tile)
        {
            await sidesCreator.SetupSidesAwaitableAsync(initializer, GridPosition, true, tile);
            initializer.FloorSide.Renderer = builder.RendererSource.GetTeleportFloorSideRenderer(initializer.FloorSide, builder.WallTexture, builder.TeleportTexture);
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
                logicActuatorCreator.ParseActuatorCreator(logicTileInitializer, logicSensors);
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
                RandomDecoration = t.AllowRandomDecoration,
            };

            var res = new WallIlusion(trickTileInitializer);
            SetupWallIllusionSidesAsync(trickTileInitializer, t.AllowRandomDecoration, res);
            res.Renderer = builder.RendererSource.GetWallIllusionTileRenderer(res, builder.WallTexture);

            initializer = trickTileInitializer;
            return res;
        }

        private async void SetupWallIllusionSidesAsync(WallIlusionInitializer initializer, bool randomDecoration, ITile tile)
        {
            await sidesCreator.SetupSidesAwaitableAsync(initializer, GridPosition, randomDecoration, tile);

            var trickSides = MapDirection.Sides.Except(initializer.WallSides.Select(x => x.Face))
                .Select(x =>
                {
                    var decoration = randomDecoration ? builder.RandomWallDecoration : null;
                    var res = new TileSide(x, decoration != null);
                    res.Renderer = builder.RendererSource.GetWallIllusionTileSideRenderer(res, builder.WallTexture, decoration);
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
            res.Renderer = builder.RendererSource.GetStairsTileRenderer(res, builder.WallTexture);
            this.initializer = stairsInitializer;
            return res;
        }

        private TileInfo<StairsTileData> FindStairs(Point pos, int level)
        {
            var map = builder.Data.Maps[level];

            var stairs = map[pos.X, pos.Y];

            if (stairs.GetType() == typeof(StairsTileData))
                return new TileInfo<StairsTileData> { Position = pos, Tile = (StairsTileData)stairs };
            else
                throw new InvalidOperationException("Should not be possible");
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
