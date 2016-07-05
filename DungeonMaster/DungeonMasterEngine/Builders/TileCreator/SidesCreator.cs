using System;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders.ActuatorCreator;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders.TileCreator
{
    public class SidesCreator
    {
        private readonly LegacyMapBuilder builder;
        private readonly WallActuatorCreator wallCreator;
        private readonly FloorActuatorCreator floorCreator;

        public SidesCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
            wallCreator = new WallActuatorCreator(builder);
            floorCreator = new FloorActuatorCreator(builder);
        }

        public async void SetupSidesAsync(FloorInitializer initalizer, Point pos, ITile tile)
        {
            await SetupSidesAwaitableAsync(initalizer, pos, tile);
        }

        public virtual async Task SetupSidesAwaitableAsync(FloorInitializer initalizer, Point pos, ITile tile)
        {
            var sides = await Task.WhenAll(MapDirection.Sides
                            .Select(d => Tuple.Create(d, builder.CurrentMap.GetTileData(pos + d)))
                            .Where(t => t.Item2 == null || t.Item2.GetType() == typeof(WallTileData))
                            .Select(async t => await CreateWallSide(t.Item1, (WallTileData)t.Item2, pos))
                            .ToArray());

            initalizer.WallSides = sides
                .Concat(new[] { GetCeelingSide() })
                .ToArray();

            initalizer.FloorSide = await GetFloorSide(pos, tile);
        }

        private TileSide GetCeelingSide()
        {
            var ceeling = new TileSide(MapDirection.Up);
            ceeling.Renderer = builder.RendererSource.GetCeelingRenderer(ceeling, builder.WallTexture);
            return ceeling;
        }

        private IGrabableItem SetupItem(ItemData itemData, ITile tile)
        {
            var item = builder.ItemCreator.CreateItem(itemData);
            item.SetLocationNoEvents(new FourthSpaceRouteElement(null, tile));//space is overridden by floor itself
            return item;
        }

        private async Task<FloorTileSide> GetFloorSide(Point point, ITile tile)
        {
            var tileData = builder.CurrentMap[point.X, point.Y];

            var topleft = tileData.GrabableItems.Where(x => x.TilePosition == TilePosition.North_TopLeft).Select(x => SetupItem(x, tile));
            var topRight = tileData.GrabableItems.Where(x => x.TilePosition == TilePosition.East_TopRight).Select(x => SetupItem(x, tile));
            var bottomLeft = tileData.GrabableItems.Where(x => x.TilePosition == TilePosition.South_BottomLeft).Select(x => SetupItem(x, tile));
            var bottomRifgh = tileData.GrabableItems.Where(x => x.TilePosition == TilePosition.West_BottomRight).Select(x => SetupItem(x, tile));

            var floorTile = tileData as FloorTileData;
            Texture2D texture = null;
            if (floorTile != null)
                texture = floorTile.RandomDecoration != null ? builder.FloorTextures[floorTile.RandomDecoration.Value] : null;

            if (!tileData.Actuators.Any())
            {
                var floor = new FloorTileSide(texture != null, MapDirection.Down, topleft, topRight, bottomLeft, bottomRifgh);
                floor.Renderer = builder.RendererSource.GetFloorRenderer(floor, builder.WallTexture, texture);
                return floor;
            }
            else
            {
                var floor = new ActuatorFloorTileSide(await floorCreator.GetFloorActuator(tileData.Actuators), texture != null, MapDirection.Down, topleft, topRight, bottomLeft, bottomRifgh);
                floor.Renderer = builder.RendererSource.GetActuatorFloorRenderer(floor, builder.WallTexture, texture);
                return floor;
            }
        }

        private async Task<TileSide> CreateWallSide(MapDirection wallDirection, WallTileData wall, Point pos)
        {
            ActuatorItemData[] sensorsData;
            if (wall != null)
                sensorsData = wall.Actuators.Where(a => a.TilePosition == wallDirection.Opposite.ToTilePosition()).ToArray();
            else
                sensorsData = new ActuatorItemData[0];

            TextDataItem textData = wall?.TextTags.FirstOrDefault(x => x.TilePosition == wallDirection.Opposite.ToTilePosition());

            int? randomTexture;
            if (!sensorsData.Any() && AllowedRandomDecoration(wallDirection, wall, out randomTexture))
                sensorsData = new[]
                {
                    new ActuatorItemData
                    {
                        ActuatorType = 0,
                        IsLocal = true,
                        Decoration = randomTexture.Value + 1
                    }
                };

            if (textData != null)
            {
                var res = new TextTileSide(wallDirection, textData.IsVisible, textData.Text);
                res.Renderer = builder.RendererSource.GetTextSideRenderer(res, builder.WallTexture);
                return res;
            }
            else if (!sensorsData.Any())
            {
                var res = new TileSide(wallDirection);
                res.Renderer = builder.RendererSource.GetWallSideRenderer(res, builder.WallTexture);
                return res;
            }
            else
            {
                var items = wall.GrabableItems
                    .Select(builder.ItemCreator.CreateItem)
                    .ToList();

                var res = new ActuatorWallTileSide(await wallCreator.ParseActuatorX(sensorsData, items, pos), wallDirection);
                res.Renderer = builder.RendererSource.GetActuatorWallSideRenderer(res, builder.WallTexture);
                return res;
            }
        }

        private bool AllowedRandomDecoration(MapDirection direction, WallTileData data, out int? decorationNumber)
        {
            decorationNumber = null;
            if (data != null)
            {
                if (direction == MapDirection.North)
                    decorationNumber = data.NorthRandomDecoration;
                if (direction == MapDirection.South)
                    decorationNumber = data.SouthRandomDecoration;
                if (direction == MapDirection.East)
                    decorationNumber = data.EastRandomDecoration;
                if (direction == MapDirection.West)
                    decorationNumber = data.WestRandomDecoration;
            }

            return decorationNumber != null;
        }



    }
}