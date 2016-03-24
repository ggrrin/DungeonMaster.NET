using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Items;
using DungeonMasterEngine.Tiles;
using DungeonMasterParser;
using DungeonMasterParser.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonMasterEngine.Builders
{
    public partial class OldDungeonBuilder : IDungonBuilder
    {
        class TileCreator : ITileCreator<Tile>
        {
            private OldDungeonBuilder builder;
            private int level;
            private Vector3 position;

            public IDictionary<Point, Tile> exitTriggers { get; private set; } = new Dictionary<Point, Tile>();


            private Color[] miniMapData;

            private Texture2D texture;
            public Texture2D MiniMap
            {
                get
                {
                    texture.SetData(miniMapData);
                    return texture;
                }
            }

            public IEnumerable<TileInfo<DungeonMasterParser.Tile>> Successors { get; private set; }

            public TileCreator(OldDungeonBuilder d, int level)
            {
                builder = d;
                this.level = level;


                texture = new Texture2D(ResourceProvider.Instance.Device, builder.map.OffsetX + builder.map.Width, builder.map.OffsetY + builder.map.Height);
                miniMapData = new Color[texture.Width * texture.Height];
            }

            private void SetMinimapTile(Color color)
            {
                miniMapData[(int)position.Z * texture.Width + (int)position.X] = color;
            }

            public Tile GetTile(TileInfo<DungeonMasterParser.Tile> tileInfo)
            {
                Successors = Enumerable.Empty<TileInfo<DungeonMasterParser.Tile>>();
                position = new Vector3(tileInfo.Position.X, -level, tileInfo.Position.Y);
                return tileInfo.Tile.GetTile(this);
            }

            public Tile GetTile(PitTile t)
            {
                SetMinimapTile(Color.Orange);

                return new Pit(position);
            }

            public Tile GetTile(TeleporterTile t)
            {
                SetMinimapTile(Color.Blue);

                var teleport = (TeleporterItem)t.Items.Find(x => x.GetType() == typeof(TeleporterItem));
                teleport.Processed = true;

                var destinationPosition = teleport.DestinationPosition.ToAbsolutePosition(builder.data.Maps[teleport.MapIndex]);

                if (teleport.MapIndex == level)
                    Successors = new List<TileInfo<DungeonMasterParser.Tile>>
                    {
                        new TileInfo<DungeonMasterParser.Tile> {
                            Position = destinationPosition,
                            Tile = builder.map[destinationPosition.X, destinationPosition.Y]
                        }
                    };

                return new Tiles.Teleport(position, teleport.MapIndex, destinationPosition, t.IsOpen, t.IsVisible, GetTeleportScopeType(teleport.Scope));
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
                        return new TypeConstrain(typeof(Items.GrabableItem));
                    case TeleportScope.ItemsOrParty:
                        return new OrConstrain(new List<IConstrain>
                        {
                            new TypeConstrain(typeof(Items.GrabableItem)),
                            new PartyConstrain()
                        });
                    default: throw new InvalidOperationException();

                }
            }

            public Tile GetTile(WallTile t)
            {
                throw new InvalidOperationException();
                //SetMinimapTile(Color.Brown);
            }

            public Tile GetTile(TrickTile t)
            {
                SetMinimapTile(Color.Green);
                return new WallIlusion(position);
            }

            private TileInfo<DungeonMasterParser.StairsTile> FindStairs(Point pos, int level)
            {
                var map = builder.data.Maps[level];

                var stairs = map[pos.X, pos.Y];

                if (stairs.GetType() == typeof(StairsTile))
                {
                    return new TileInfo<StairsTile> { Position = pos, Tile = (StairsTile)stairs };
                }
                else//TODO shouldnt be necesarry
                {
                    var k = builder.GetNeigbourTiles(pos, map);

                    var res = k.Find(x => x.Tile.GetType() == typeof(Stairs));
                    if (res.Tile == null)
                        throw new InvalidOperationException("Invalid map format");
                    else
                        return new TileInfo<StairsTile> { Position = res.Position, Tile = (StairsTile)res.Tile };
                }
            }


            public Tile GetTile(StairsTile t)
            {
                SetMinimapTile(Color.Yellow);

                TileInfo<StairsTile> stairs;
                if (t.Direction == VerticalDirection.Up)
                {
                    stairs = FindStairs(position.ToGrid(), level - 1);
                    return new Stairs(position);//ghostStairs = to connect levels
                }
                else// take care of showing stairs in right direction
                {
                    stairs = FindStairs(position.ToGrid(), level + 1);

                    return new Stairs(position, t.Orientation != Orientation.NorthSouth, t.Orientation != stairs.Tile.Orientation);
                }
            }

            public Tile GetTile(FloorTile t)
            {
                SetMinimapTile(Color.White);
                return new Floor(position/*, faces*/);
            }

            public Tile GetTile(DoorTile t)
            {
                SetMinimapTile(Color.Purple);
                //foreach(var k in from i in t.GetItems(builder.data) select (i))
                //{
                //    from j in t.GetItems(builder.data) where j.TilePosition == TilePosition.North_TopLeft || j.TilePosition == TilePosition.West_BottomRight select new { Position = j.TilePosition, ahojky = j.NextObjectID, behojky = j.ToString() };
                //}

                DoorItem door = (from i in t.Items where i.GetType() == typeof(DoorItem) select (DoorItem)i).FirstOrDefault();
                door.Processed = true;

                return new Tiles.Gateway(position, t.Orientation == Orientation.WestEast, t.State == DoorState.Open || t.State == DoorState.Bashed, (Door)builder.itemCreator.GetItem(door));
            }
        }
    }
}
