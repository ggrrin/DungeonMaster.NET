using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    class WallCreator
    {
        private readonly LegacyMapBuilder builder;

        public WallCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public async void SetupSides(FloorInitializer initialier, Point pos)
        {
            //var sides =
            //    MapDirection.AllSides
            //    .Select(d => Tuple.Create(d, builder.CurrentMap.GetTileData(pos + d)))
            //    .Where(t =>  t.Item2?.GetType() == typeof(WallTileData))
            //    .Select(side =>
            //    {
            //        var pos = CurrentTile.GridPosition + side;
            //        var wall = builder.CurrentMap.GetTileData(pos); //get appropriate WallData
            //    return wall == null ? null : new Tuple<TileData, Point, IReadOnlyList<ActuatorItemData>>(wall, pos,
            //            wall.Actuators.Where(x => x.TilePosition == side.Opposite.ToTilePosition())
            //            .ToArray());//select appropriate side
            //    })

        }

        private async Task<TileSide> CreateWallSide(MapDirection wallDirection, WallTileData wall)
        {
            var sensorsData = wall.Actuators
                .Where(a => a.TilePosition == wallDirection.Opposite.ToTilePosition())
                .ToArray();

            if (!sensorsData.Any())
            {
                var res = new TileSide(wallDirection, AllowedRandomDecoration(wallDirection, wall));
                res.Renderer = source.GetRenderer(res);
                return res;
            }
            else
            {
                var res = new SubItemTileSide(await ParseActuatorX(sensorsData), wallDirection);
                res.Renderer = source.GetRenderer(res);
                return res;
            }
        }

        private IGraphicSource source;


        private bool AllowedRandomDecoration(MapDirection direction, WallTileData data)
        {
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

        private async Task<IActuatorX> ParseActuatorX(IEnumerable<ActuatorItemData> data)
        {
            var sensors = await Task.WhenAll(data.Select(async x => await ParseSensor(x)));
            var res = new ActuatorX(sensors);
            res.Renderer = source.GetRenderer(res);
            return res;
        }

        private async Task<Sensor> ParseSensor(ActuatorItemData data)
        {
            var local = data.ActionLocation as LocalTarget;
            var remote = data.ActionLocation as RemoteTarget;
            var initializer = new SensorInitializer
            {
                Audible = data.HasSoundEffect,
                Effect = (SensorEffect)data.Action,
                LocalEffect = data.IsLocal,
                ExperienceGain = local?.ExperienceGain ?? false,
                Rotate = local?.RotateAutors ?? false,
                OnceOnly = data.IsOnceOnly,
                RevertEffect = data.IsRevertable,
                Specifer = (EarthSides?)remote?.Position.Direction ?? EarthSides.C00_CELL_NORTHWEST,
                TimeDelay = 1000 / 6 * data.ActionDelay,
                Data = builder.GetItemFactory(data.Data),
                TargetTile = await builder.GetTargetTile(remote?.Position.Position.ToAbsolutePosition(builder.CurrentMap)),
            };

            switch (data.ActuatorType)
            {
                case 1:
                    return new Sensor1(initializer);
                case 2:
                    return new Sensor2(initializer);
                case 3:
                    return new Sensor3(initializer);
                case 4:
                    return new Sensor4(initializer);
                case 11:
                    return new Sensor11(initializer);
                case 12:
                    return new Sensor12(initializer);
                case 13:
                    return new Sensor13(initializer);
                case 16:
                    return new Sensor16(initializer);
                case 17:
                    return new Sensor17(initializer);
                case 127:
                    return new Sensor127(initializer);
                default:
                    return null;
            }
        }
    }

    interface IGraphicSource
    {
        IRenderer GetRenderer(ActuatorX res);
        IRenderer GetRenderer(TileSide side);
    }
}