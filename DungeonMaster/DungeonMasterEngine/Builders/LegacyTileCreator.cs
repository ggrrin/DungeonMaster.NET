using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Tiles;
using GrabableItem = DungeonMasterEngine.DungeonContent.Items.GrabableItems.GrabableItem;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public class LegacyTileCreator : ITileCreator<Tile>
    {
        private readonly LegacyMapBuilder builder;
        private readonly Color[] miniMapData;
        private readonly Texture2D texture;
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

        public IEnumerable<TileInfo<TileData>> Successors { get; private set; }

        public LegacyTileCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;

            texture = new Texture2D(ResourceProvider.Instance.Device, this.builder.CurrentMap.OffsetX + this.builder.CurrentMap.Width, this.builder.CurrentMap.OffsetY + this.builder.CurrentMap.Height);
            miniMapData = new Color[texture.Width * texture.Height];
            sidesCreator = new SidesCreator(builder);
        }

        private void SetMinimapTile(Color color) => miniMapData[(int)tilePosition.Z * texture.Width + (int)tilePosition.X] = color;

        public Tile GetTile(TileInfo<TileData> tileInfo)
        {
            Successors = Enumerable.Empty<TileInfo<TileData>>();//reset sucessors
            tilePosition = new Vector3(tileInfo.Position.X, -level, tileInfo.Position.Y);
            GridPosition = tileInfo.Position;
            var tile = tileInfo.Tile.GetTile(this);
            if (initalizer != null)
            {
                initalizer.GridPosition = GridPosition;
                builder.TileInitializers.Add(initalizer);
                initalizer = null;
            }
            return tile;
        }

        public Point GridPosition { get; private set; }

        private readonly SidesCreator sidesCreator;
        private TileInitializer initalizer;

        public Tile GetTile(FloorTileData t)
        {
            SetMinimapTile(Color.White);

            var initalizer = new FloorInitializer();
            sidesCreator.SetupSides(initalizer, GridPosition, t.AllowRandomDecoration);
            var res = new FloorTile(initalizer);
            res.Renderer = builder.RendererSource.GetTileRenderer(res);
            this.initalizer = initalizer;
            return res;
        }

        public Tile GetTile(DoorTileData t)
        {
            if (t.Door == null)
                throw new ArgumentNullException("Invalid map format. Door item should be at door tile.");

            SetMinimapTile(Color.Purple);

            var initalizer = new DoorInitializer { Door = CreateDoor(t) };
            initalizer.HasButton = t.Door.HasButton;
            
            sidesCreator.SetupSides(initalizer, GridPosition, false);

            initalizer.Direction = t.Orientation == Orientation.NorthSouth ? MapDirection.North : MapDirection.East;

            var res = new DoorTile(initalizer);
            res.Renderer = builder.RendererSource.GetDoorTileRenderer(res, builder.WallTexture, builder.DoorButtonTexture);
            this.initalizer = initalizer;
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

            return null;
        }

        public Tile GetTile(TeleporterTileData t)
        {
            SetMinimapTile(Color.Blue);

            if (t.Teleport != null)
            {
                t.Teleport.Processed = true;

                var destinationPosition = t.Teleport.DestinationPosition.ToAbsolutePosition(builder.Data.Maps[t.Teleport.MapIndex]);

                if (t.Teleport.MapIndex == level)
                {
                    Successors = new[] {new TileInfo<TileData>
                    {
                        Position = destinationPosition,
                        Tile = builder.CurrentMap[destinationPosition.X, destinationPosition.Y]
                    }};
                }

                return null;
            }
            else
            {
                throw new InvalidOperationException("Invalid map format. Teleport tile has to have teleport");
            }
        }

        public Tile GetTile(WallTileData t)
        {
            return null;
        }

        public Tile GetTile(TrickTileData t)
        {
            SetMinimapTile(Color.Green);
            return null;
        }

        public Tile GetTile(StairsTileData t)
        {
            SetMinimapTile(Color.Yellow);

            TileInfo<StairsTileData> stairs;
            if (t.Direction == VerticalDirection.Up)
            {
                stairs = FindStairs(tilePosition.ToGrid(), level - 1);
                //return new Stairs(tilePosition);//ghostStairs = to connect levels
            }
            else// take care of showing stairs in right direction
            {
                stairs = FindStairs(tilePosition.ToGrid(), level + 1);
                //return new Stairs(tilePosition, t.Orientation != Orientation.NorthSouth, t.Orientation != stairs.Tile.Orientation);

            }
            return null;
        }

        private TileInfo<StairsTileData> FindStairs(Point pos, int level)
        {
            var map = builder.Data.Maps[level];

            var stairs = map[pos.X, pos.Y];

            if (stairs.GetType() == typeof(StairsTileData))
            {
                return new TileInfo<StairsTileData> { Position = pos, Tile = (StairsTileData)stairs };
            }
            else//TODO shouldnt be necesarry
            {
                var k = builder.GetNeigbourTiles(pos, map);

                var res = k.FirstOrDefault(x => x.Tile.GetType() == typeof(Stairs));
                if (res.Tile == null)
                    throw new InvalidOperationException("Invalid CurrentMap format");
                else
                    return new TileInfo<StairsTileData> { Position = res.Position, Tile = (StairsTileData)res.Tile };
            }
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
