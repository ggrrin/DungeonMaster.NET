using System;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders.ActuatorCreators;
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

namespace DungeonMasterEngine.Builders.TileCreators
{

    public interface ISidesCreator
    {
        void SetupSidesAsync(FloorInitializer initalizer, Point pos, ITile tile);

        Task SetupSidesAwaitableAsync(FloorInitializer initalizer, Point pos, ITile tile);
    }

    public class SidesCreator : ISidesCreator
    {
        protected readonly LegacyMapBuilder builder;
        protected readonly IWallActuatorCreator wallCreator;
        protected readonly IFloorActuatorCreator floorCreator;

        public SidesCreator(LegacyMapBuilder builder, IWallActuatorCreator wallCreator, IFloorActuatorCreator floorCreator)
        {
            this.builder = builder;
            this.wallCreator = wallCreator;
            this.floorCreator = floorCreator;
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

            //int level = builder.CurrentLevelIndex - 1;
            //if (level >= 0 && builder.Data.Maps[level].GetTileData(pos) is PitTileData)
            //{
            //    initalizer.WallSides = sides;
            //}
            //else
            //{
                initalizer.WallSides = sides
                    .Concat(new[] { GetCeelingSide() })
                    .ToArray();
            //}

            initalizer.FloorSide = await GetFloorSide(pos, tile);
        }

        protected virtual TileSide GetCeelingSide()
        {
            var ceeling = new TileSide(MapDirection.Up);
            ceeling.Renderer = builder.Factories.RenderersSource.GetCeelingRenderer(ceeling, builder.WallTexture);
            return ceeling;
        }

        protected IGrabableItem SetupItem(ItemData itemData, ITile tile)
        {
            var item = builder.ItemCreator.CreateItem(itemData);
            item.SetLocationNoEvents(new FourthSpaceRouteElement(null, tile));//space is overridden by floor itself
            return item;
        }

        protected virtual async Task<FloorTileSide> GetFloorSide(Point point, ITile tile)
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
                floor.Renderer = builder.Factories.RenderersSource.GetFloorRenderer(floor, builder.WallTexture, texture);
                return floor;
            }
            else
            {
                var floor = new ActuatorFloorTileSide(await floorCreator.GetFloorActuator(tileData.Actuators), texture != null, MapDirection.Down, topleft, topRight, bottomLeft, bottomRifgh);
                floor.Renderer = builder.Factories.RenderersSource.GetActuatorFloorRenderer(floor, builder.WallTexture, texture);
                return floor;
            }
        }

        protected virtual async Task<TileSide> CreateWallSide(MapDirection wallDirection, WallTileData wall, Point pos)
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
                res.Renderer = builder.Factories.RenderersSource.GetTextSideRenderer(res, builder.WallTexture);
                return res;
            }
            else if (!sensorsData.Any())
            {
                var res = new TileSide(wallDirection);
                res.Renderer = builder.Factories.RenderersSource.GetWallSideRenderer(res, builder.WallTexture);
                return res;
            }
            else
            {
                var items = wall.GrabableItems
                    .Select(builder.ItemCreator.CreateItem)
                    .ToList();

                var res = new ActuatorWallTileSide(await wallCreator.ParseActuatorX(sensorsData, items, pos), wallDirection);
                res.Renderer = builder.Factories.RenderersSource.GetActuatorWallSideRenderer(res, builder.WallTexture);
                return res;
            }
        }

        protected bool AllowedRandomDecoration(MapDirection direction, WallTileData data, out int? decorationNumber)
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