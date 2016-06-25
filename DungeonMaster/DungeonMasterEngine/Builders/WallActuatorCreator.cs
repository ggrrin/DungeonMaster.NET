using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.WallSensors;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public class WallActuatorCreator :ActuatorCreatorBase
    {
        public WallActuatorCreator(LegacyMapBuilder builder) : base(builder) { } 

        public async Task<IActuatorX> ParseActuatorX(IEnumerable<ActuatorItemData> data, List<IGrabableItem> items, Point pos)
        {
            var sensors = await Task.WhenAll(data
                .Where(x => x.ActuatorType != 5 && x.ActuatorType != 6)
                .Select(async x => await ParseSensor(x, items, pos)));
            var res = new WallActuator(sensors);
            res.Renderer = builder.RendererSource.GetWallActuatorRenderer(res);
            return res;
        }

        private async Task<WallSensor> ParseSensor(ActuatorItemData data, List<IGrabableItem> items, Point pos)
        {
            SensorInitializer<IActuatorX> initializer = new SensorInitializer<IActuatorX>();

            switch (data.ActuatorType)
            {
                case 10:
                //TODO

                case 0:
                    await SetupInitializer(initializer, data);
                    initializer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor0(initializer);
                case 1:
                    await SetupInitializer(initializer, data);
                    initializer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor1(initializer);
                case 2:
                    var sensor2initalizer = new ItemConstrainSensorInitalizer<IActuatorX> { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor2initalizer, data);
                    sensor2initalizer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor2(sensor2initalizer);
                case 3:
                    var sensor3initalizer = new ItemConstrainSensorInitalizer<IActuatorX> { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor3initalizer, data);
                    sensor3initalizer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor3(sensor3initalizer);
                case 4:
                    var sensor4initalizer = new ItemConstrainSensorInitalizer<IActuatorX> { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor4initalizer, data);
                    sensor4initalizer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor4(sensor4initalizer);
                case 11:
                    var sensor11initalizer = new ItemConstrainSensorInitalizer<IActuatorX> { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor11initalizer, data);
                    sensor11initalizer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor11(sensor11initalizer);
                case 12:
                    var sensor12initalizer = new ItemConstrainSensorInitalizer<IActuatorX> { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor12initalizer, data);
                    sensor12initalizer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor12(sensor12initalizer);
                case 13:
                    var sensor13initalizer = new StorageSensorInitializer<IActuatorX> { StoredItem = GetStoredObject(items, data.Data), Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor13initalizer, data);
                    sensor13initalizer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor13(sensor13initalizer);
                case 16:
                    var sensor16initalizer = new StorageSensorInitializer<IActuatorX> { StoredItem = GetStoredObject(items, data.Data), Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor16initalizer, data);
                    sensor16initalizer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor16(sensor16initalizer);
                case 17:
                    var sensor17initalizer = new ItemConstrainSensorInitalizer<IActuatorX> { Data = builder.GetItemFactory(data.Data) };
                    await SetupInitializer(sensor17initalizer, data);
                    sensor17initalizer.Graphics = CreateWallDecoration(data.Decoration - 1, items);
                    return new Sensor17(sensor17initalizer);
                case 127:
                    return await CreateChampionActuator(data, items, pos);
                default:
                    throw new InvalidOperationException();
            }
        }


        private async Task<WallSensor> CreateChampionActuator(ActuatorItemData data, List<IGrabableItem> items, Point pos)
        {
            var face = builder.ChampionTextures[data.Data];
            var sensor127initializer = new ChampionSensorInitializer
            {
                Champion = new ChampionFactory(builder).GetChampion(pos, face),
                GridPosition = pos,
            };
            await SetupInitializer(sensor127initializer, data);
            var dec = new ChampionDecoration(true);
            sensor127initializer.Graphics = dec;
            sensor127initializer.Graphics.Renderer = builder.RendererSource.GetChampionRenderer(dec, builder.WallTextures[data.Decoration - 1], face);
            return new Sensor127(sensor127initializer);
        }

        private IGrabableItem GetStoredObject(List<IGrabableItem> items, int data)
        {
            var factory = builder.GetItemFactory(data);

            var res = items.Single(i => i.Factory == factory);
            items.Remove(res);
            return res;
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
