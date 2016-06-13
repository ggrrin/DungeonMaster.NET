using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
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

        public async void SetupSides(FloorInitializer initalizer, Point pos, bool allowRandomDecoration)
        {
            var sides = await Task.WhenAll(
                MapDirection.Sides
                .Select(d => Tuple.Create(d, builder.CurrentMap.GetTileData(pos + d)))
                .Where(t => t.Item2 == null || t.Item2.GetType() == typeof(WallTileData))
                .Select(async t => await CreateWallSide(t.Item1, (WallTileData)t.Item2, pos))
                .ToArray());

            initalizer.WallSides = sides
                .Concat(new[] { GetCeelingSide() })
                .ToArray();

            initalizer.FloorSide = await GetFloorSide(pos, allowRandomDecoration);
        }

        private TileSide GetCeelingSide()
        {
            var ceeling = new TileSide(MapDirection.Up, false);
            ceeling.Renderer = builder.RendererSource.GetCeelingRenderer(ceeling, builder.WallTexture);
            return ceeling;
        }

        private async Task<FloorTileSide> GetFloorSide(Point point, bool allowRandomDecoration)
        {
            var tile = builder.CurrentMap[point.X, point.Y];
            Texture2D texture = allowRandomDecoration ? builder.RandomFloorDecoration : null;

            var topleft = tile.GrabableItems.Where(x => x.TilePosition == TilePosition.North_TopLeft).Select(builder.ItemCreator.CreateItem);
            var topRight = tile.GrabableItems.Where(x => x.TilePosition == TilePosition.East_TopRight).Select(builder.ItemCreator.CreateItem);
            var bottomLeft= tile.GrabableItems.Where(x => x.TilePosition == TilePosition.South_BottomLeft).Select(builder.ItemCreator.CreateItem);
            var bottomRifgh = tile.GrabableItems.Where(x => x.TilePosition == TilePosition.West_BottomRight).Select(builder.ItemCreator.CreateItem);

            if (!tile.Actuators.Any())
            {
                var floor = new FloorTileSide(texture != null, MapDirection.Down, topleft, topRight, bottomLeft, bottomRifgh);
                floor.Renderer = builder.RendererSource.GetFloorRenderer(floor, builder.WallTexture, texture);
                return floor;
            }
            else
            {
                var floor = new ActuatorFloorTileSide(await floorCreator.GetFloorActuator(tile.Actuators), texture != null, MapDirection.Down, topleft, topRight, bottomLeft, bottomRifgh);
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

            if (textData != null)
            {
                var res = new TextTileSide(wallDirection, textData.IsVisible, textData.Text);
                res.Renderer = builder.RendererSource.GetTextSideRenderer(res, builder.WallTexture);
                return res;
            }
            else if (!sensorsData.Any())
            {
                Texture2D randomTexture;
                var res = new TileSide(wallDirection, AllowedRandomDecoration(wallDirection, wall, out randomTexture));
                res.Renderer = builder.RendererSource.GetWallSideRenderer(res, builder.WallTexture, randomTexture);
                return res;
            }
            else
            {
                var items = wall.GrabableItems
                    .Select(builder.ItemCreator.CreateItem)
                    .ToList();

                var res = new ActuatorWallTileSide(await wallCreator.ParseActuatorX(sensorsData, items, pos), wallDirection);
                res.Renderer = builder.RendererSource.GetActuatorWallSideRenderer(res, builder.WallTexture, null);
                return res;
            }
        }

        private bool AllowedRandomDecoration(MapDirection direction, WallTileData data, out Texture2D texture)
        {
            bool res = false;
            if (data != null)
            {
                if (direction == MapDirection.North)
                    res = data.AllowNorthRandomDecoration;
                if (direction == MapDirection.South)
                    res = data.AllowSouthRandomDecoration;
                if (direction == MapDirection.East)
                    res = data.AllowEastRandomDecoration;
                if (direction == MapDirection.West)
                    res = data.AllowWestRandomDecoration;
            }

            if (res)
            {
                texture = builder.RandomWallDecoration;
                return texture != null;
            }
            else
            {
                texture = null;
                return false;
            }
        }



    }
}