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

        public SidesCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public async void SetupSides(FloorInitializer initalizer, Point pos, bool allowRandomDecoration)
        {
            var sides = await Task.WhenAll(
                MapDirection.Sides
                .Select(d => Tuple.Create(d, builder.CurrentMap.GetTileData(pos + d)))
                .Where(t => t.Item2 == null || t.Item2.GetType() == typeof(WallTileData))
                .Select(async t => await CreateWallSide(t.Item1, (WallTileData)t.Item2))
                .ToArray());

            initalizer.Sides = sides
                .Concat(SetupCurrentTile(pos, allowRandomDecoration) )
                .ToArray();
        }

        private IEnumerable<TileSide> SetupCurrentTile(Point point, bool allowRandomDecoration)
        {
            var tile = builder.CurrentMap[point.X, point.Y];

            var ceeling = new TileSide(MapDirection.Up, false);
            ceeling.Renderer = builder.RendererSource.GetCeelingRenderer(ceeling, builder.WallTexture);
            yield return ceeling;

            Texture2D texture = allowRandomDecoration ? builder.RandomFloorDecoration : null;

            var floor = new FloorTileSide(texture != null, MapDirection.Down,
                tile.GrabableItems
                    .Where(x => x.TilePosition == TilePosition.North_TopLeft)
                    .Select(builder.ItemCreator.CreateItem),
                tile.GrabableItems
                    .Where(x => x.TilePosition == TilePosition.East_TopRight)
                    .Select(builder.ItemCreator.CreateItem),
                tile.GrabableItems
                    .Where(x => x.TilePosition == TilePosition.South_BottomLeft)
                    .Select(builder.ItemCreator.CreateItem),
                tile.GrabableItems
                    .Where(x => x.TilePosition == TilePosition.West_BottomRight)
                    .Select(builder.ItemCreator.CreateItem)
                );
            floor.Renderer = builder.RendererSource.GetFloorRenderer(floor, builder.WallTexture, texture);
            yield return floor;
        }

        private async Task<TileSide> CreateWallSide(MapDirection wallDirection, WallTileData wall)
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
                res.Renderer = builder.RendererSource.GetRenderer(res, builder.WallTexture, randomTexture);
                return res;
            }
            else
            {
                var items = wall.GrabableItems
                    .Select(builder.ItemCreator.CreateItem)
                    .ToList();

                var res = new ActuatorWallTileSide(await ParseActuatorX(sensorsData, items), wallDirection);
                res.Renderer = builder.RendererSource.GetRenderer(res, builder.WallTexture, null);
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

        private async Task<IActuatorX> ParseActuatorX(IEnumerable<ActuatorItemData> data, List<IGrabableItem> items)
        {
            var sensors = await Task.WhenAll(data.Select(async x => await ParseSensor(x, items)));
            var res = new ActuatorX(sensors);
            res.Renderer = builder.RendererSource.GetRenderer(res);
            return res;
        }

        private async Task<Sensor> ParseSensor(ActuatorItemData data, List<IGrabableItem> items)
        {
            SensorInitializer initializer = new SensorInitializer();

            switch (data.ActuatorType)
            {
                case 0:
                    await SetupInitializer(initializer, data, items);
                    return new Sensor0(initializer);
                case 1:
                    await SetupInitializer(initializer, data, items);
                    return new Sensor1(initializer);
                case 2:
                    var sensor2initalizer = new ItemConstrainSensorInitalizer { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor2initalizer, data, items);
                    return new Sensor2(sensor2initalizer);
                case 3:
                    var sensor3initalizer = new ItemConstrainSensorInitalizer { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor3initalizer, data, items);
                    return new Sensor3(sensor3initalizer);
                case 4:
                    var sensor4initalizer = new ItemConstrainSensorInitalizer { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor4initalizer, data, items);
                    return new Sensor4(sensor4initalizer);
                case 11:
                    var sensor11initalizer = new ItemConstrainSensorInitalizer { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor11initalizer, data, items);
                    return new Sensor11(sensor11initalizer);
                case 12:
                    var sensor12initalizer = new ItemConstrainSensorInitalizer { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor12initalizer, data, items);
                    return new Sensor12(sensor12initalizer);
                case 13:
                    var sensor13initalizer = new StorageSensorInitializer { StoredItem = GetStoredObject(items, data.Data), Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor13initalizer, data, items);
                    return new Sensor13(sensor13initalizer);
                case 16:
                    var sensor16initalizer = new StorageSensorInitializer { StoredItem = GetStoredObject(items, data.Data), Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor16initalizer, data, items);
                    return new Sensor16(sensor16initalizer);
                case 17:
                    var sensor17initalizer = new ItemConstrainSensorInitalizer { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor17initalizer, data, items);
                    return new Sensor17(sensor17initalizer);
                case 127:
                    await SetupInitializer(initializer, data, items);
                    return new Sensor127(initializer);
                default:
                    throw new InvalidOperationException();
            }
        }

        private IGrabableItem GetStoredObject(List<IGrabableItem> items, int data)
        {
            var factory = builder.GetItemFactory(data);

            var res = items.Single(i => i.Factory == factory);
            items.Remove(res);
            return res;
        }

        private async Task<SensorInitializer> SetupInitializer(SensorInitializer initializer, ActuatorItemData data, List<IGrabableItem> items)
        {
            var local = data.ActionLocation as LocalTarget;
            var remote = data.ActionLocation as RemoteTarget;

            initializer.Audible = data.HasSoundEffect;
            initializer.Effect = (SensorEffect)data.Action;
            initializer.LocalEffect = data.IsLocal;
            initializer.ExperienceGain = local?.ExperienceGain ?? false;
            initializer.Rotate = local?.RotateAutors ?? false;
            initializer.OnceOnly = data.IsOnceOnly;
            initializer.RevertEffect = data.IsRevertable;
            initializer.Specifer = (EarthSides?)remote?.Position.Direction ?? EarthSides.C00_CELL_NORTHWEST;
            initializer.TimeDelay = 1000 / 6 * data.ActionDelay;
            initializer.TargetTile = await builder.GetTargetTile(remote?.Position.Position.ToAbsolutePosition(builder.CurrentMap));
            initializer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
            return initializer;
        }

        private IActuatorX CreateWallDecoration(int currentIdentifer, List<IGrabableItem> items)
        {
            var descriptor = builder.CurrentMap.WallDecorations[currentIdentifer];
            var texture = builder.WallTextures[currentIdentifer];
            switch (descriptor.Type)
            {
                case GraphicsItemState.GraphicOnly:
                    var decoration = new DecorationItem();
                    decoration.Renderer = builder.RendererSource.GetDecorationRenderer(decoration, texture);
                    return decoration;

                case GraphicsItemState.Alcove:
                    var alcove = new Alcove(items);
                    items.Clear();
                    alcove.Renderer = builder.RendererSource.GetAlcoveDecoration(alcove, texture);
                    return alcove;

                case GraphicsItemState.ViAltair:
                    var altair = new ViAltairAlcove(items);
                    items.Clear();
                    altair.Renderer = builder.RendererSource.GetAlcoveDecoration(altair, texture);
                    return altair;

                case GraphicsItemState.Fountain:
                    var fountain = new Fountain();
                    fountain.Renderer = builder.RendererSource.GetFountainDecoration(fountain, texture);
                    return fountain;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}