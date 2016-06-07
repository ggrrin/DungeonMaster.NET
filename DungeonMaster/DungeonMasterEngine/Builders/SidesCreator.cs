using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
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

        public async void SetupSides(FloorInitializer initalizer, Point pos)
        {
            var sides = await Task.WhenAll(
                MapDirection.Sides
                .Select(d => Tuple.Create(d, builder.CurrentMap.GetTileData(pos + d)))
                .Where(t => t.Item2 == null || t.Item2.GetType() == typeof(WallTileData))
                .Select(async t => await CreateWallSide(t.Item1, (WallTileData)t.Item2))
                .ToArray());

            var ceeling = new TileSide(MapDirection.Up, false);
            ceeling.Renderer = builder.WallGraphicSource.GetCeelingRenderer(ceeling, builder.WallTexture);

            var floor = new FloorTileSide(MapDirection.Down, true);
            floor.Renderer = builder.WallGraphicSource.GetFloorRenderer(floor, builder.WallTexture, builder.RandomFloorDecoration);

            initalizer.Sides = sides
                .Concat(new TileSide[]
                {
                    ceeling,
                    floor
                })
                .ToArray();
        }

        private async Task<TileSide> CreateWallSide(MapDirection wallDirection, WallTileData wall)
        {
            ActuatorItemData[] sensorsData;
            if (wall != null)
                sensorsData = wall.Actuators.Where(a => a.TilePosition == wallDirection.Opposite.ToTilePosition()).ToArray();
            else
                sensorsData = new ActuatorItemData[0];

            if (!sensorsData.Any())
            {
                var res = new TileSide(wallDirection, AllowedRandomDecoration(wallDirection, wall));
                res.Renderer = builder.WallGraphicSource.GetRenderer(res, builder.WallTexture, builder.RandomWallDecoration);
                return res;
            }
            else
            {
                var items = wall.GrabableItems
                    .Select(builder.ItemCreator.CreateItem)
                    .ToList();

                var res = new ActuatorWallTileSide(await ParseActuatorX(sensorsData, items), wallDirection);
                res.Renderer = builder.WallGraphicSource.GetRenderer(res, builder.WallTexture, null);
                return res;
            }
        }

        private bool AllowedRandomDecoration(MapDirection direction, WallTileData data)
        {
            if (data == null)
                return false;

            if (direction == MapDirection.North)
                return data.AllowNorthRandomDecoration;
            if (direction == MapDirection.South)
                return data.AllowSouthRandomDecoration;
            if (direction == MapDirection.East)
                return data.AllowEastRandomDecoration;
            if (direction == MapDirection.West)
                return data.AllowWestRandomDecoration;
            throw new InvalidOperationException();
        }

        private async Task<IActuatorX> ParseActuatorX(IEnumerable<ActuatorItemData> data, List<IGrabableItem> items)
        {
            var sensors = await Task.WhenAll(data.Select(async x => await ParseSensor(x, items)));
            var res = new ActuatorX(sensors);
            res.Renderer = builder.WallGraphicSource.GetRenderer(res);
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
                    decoration.Renderer = builder.WallGraphicSource.GetDecorationRenderer(decoration, texture);
                    return decoration;

                case GraphicsItemState.Alcove:
                    var alcove = new Alcove(items);
                    items.Clear();
                    alcove.Renderer = builder.WallGraphicSource.GetAlcoveDecoration(alcove, texture);
                    return alcove;

                case GraphicsItemState.ViAltair:
                    var altair = new ViAltairAlcove(items);
                    items.Clear();
                    altair.Renderer = builder.WallGraphicSource.GetAlcoveDecoration(altair, texture);
                    return altair;

                case GraphicsItemState.Fountain:
                    var fountain = new Fountain();
                    fountain.Renderer = builder.WallGraphicSource.GetFountainDecoration(fountain, texture);
                    return fountain;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public interface IWallGraphicSource
    {
        Renderer GetRenderer(TileSide side, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetRenderer(ActuatorWallTileSide side, Texture2D wallTexture, Texture2D decorationTexture);
        Renderer GetRenderer(ActuatorX res);

        Renderer GetAlcoveDecoration(Alcove alcove, Texture2D wallTexture);

        Renderer GetDecorationRenderer(DecorationItem decoration, Texture2D wallTexture);

        Renderer GetFountainDecoration(Fountain fountain, Texture2D texture);

        Renderer GetTileRenderer(Tile tile);

        Renderer GetCeelingRenderer(TileSide ceeling, Texture2D wallTexture);

        Renderer GetFloorRenderer(FloorTileSide floorTile, Texture2D wallTexture, Texture2D decorationTexture);
    }
}