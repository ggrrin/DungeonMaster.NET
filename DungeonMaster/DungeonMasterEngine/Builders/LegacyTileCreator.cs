using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Tiles;
using GrabableItem = DungeonMasterEngine.DungeonContent.Items.GrabableItem;
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

        public IEnumerable<TileInfo<DungeonMasterParser.Tiles.Tile>> Successors { get; private set; }

        public LegacyTileCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;

            texture = new Texture2D(ResourceProvider.Instance.Device, this.builder.CurrentMap.OffsetX + this.builder.CurrentMap.Width, this.builder.CurrentMap.OffsetY + this.builder.CurrentMap.Height);
            miniMapData = new Color[texture.Width * texture.Height];
        }

        private void SetMinimapTile(Color color) => miniMapData[(int)tilePosition.Z * texture.Width + (int)tilePosition.X] = color; 

        public Tile GetTile(TileInfo<DungeonMasterParser.Tiles.Tile> tileInfo)
        {
            Successors = Enumerable.Empty<TileInfo<DungeonMasterParser.Tiles.Tile>>();//reset sucessors
            tilePosition = new Vector3(tileInfo.Position.X, -level, tileInfo.Position.Y);
            return tileInfo.Tile.GetTile(this);
        }

        public Tile GetTile(PitTile t)
        {
            SetMinimapTile(Color.Orange);

            return new Pit(tilePosition);
        }

        public Tile GetTile(TeleporterTile t)
        {
            SetMinimapTile(Color.Blue);

            var teleport = (TeleporterItem)t.Items.Find(x => x.GetType() == typeof(TeleporterItem));
            teleport.Processed = true;

            var destinationPosition = teleport.DestinationPosition.ToAbsolutePosition(builder.Data.Maps[teleport.MapIndex]);

            if (teleport.MapIndex == level)
            {
                Successors = new[] {new TileInfo<DungeonMasterParser.Tiles.Tile>
                {
                    Position = destinationPosition,
                    Tile = builder.CurrentMap[destinationPosition.X, destinationPosition.Y]
                }};
            }

            return new Teleport(tilePosition, teleport.MapIndex, destinationPosition, t.IsOpen, t.IsVisible, GetTeleportScopeType(teleport.Scope));
        }

        public Tile GetTile(WallTile t)
        {
            throw new InvalidOperationException();
        }

        public Tile GetTile(TrickTile t)
        {
            SetMinimapTile(Color.Green);
            return new WallIlusion(tilePosition, t.IsImaginary, t.IsOpen);
        }

        public Tile GetTile(StairsTile t)
        {
            SetMinimapTile(Color.Yellow);

            TileInfo<StairsTile> stairs;
            if (t.Direction == VerticalDirection.Up)
            {
                stairs = FindStairs(tilePosition.ToGrid(), level - 1);
                return new Stairs(tilePosition);//ghostStairs = to connect levels
            }
            else// take care of showing stairs in right direction
            {
                stairs = FindStairs(tilePosition.ToGrid(), level + 1);
                return new Stairs(tilePosition, t.Orientation != Orientation.NorthSouth, t.Orientation != stairs.Tile.Orientation);
            }
        }

        public Tile GetTile(FloorTile t)
        {
            SetMinimapTile(Color.White);
            return new Floor(tilePosition);
        }

        public Tile GetTile(DoorTile t)
        {
            SetMinimapTile(Color.Purple);

            DoorItem door = t.Items.OfType<DoorItem>().FirstOrDefault();
            if (door != null)
            {
                door.Processed = true;

                return new Gateway(tilePosition, t.Orientation == Orientation.WestEast, t.State == DoorState.Open || t.State == DoorState.Bashed, (Door) builder.LegacyItemCreator.GetItem(door));
            }
            else
            {
                throw new InvalidOperationException("Invalid map format. Door item should be at door tile.");
            }
        }

        private TileInfo<StairsTile> FindStairs(Point pos, int level)
        {
            var map = builder.Data.Maps[level];

            var stairs = map[pos.X, pos.Y];

            if (stairs.GetType() == typeof(StairsTile))
            {
                return new TileInfo<StairsTile> { Position = pos, Tile = (StairsTile)stairs };
            }
            else//TODO shouldnt be necesarry
            {
                var k = builder.GetNeigbourTiles(pos, map);

                var res = k.FirstOrDefault(x => x.Tile.GetType() == typeof(Stairs));
                if (res.Tile == null)
                    throw new InvalidOperationException("Invalid CurrentMap format");
                else
                    return new TileInfo<StairsTile> { Position = res.Position, Tile = (StairsTile)res.Tile };
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
