using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Actuators.Wall.FloorSensors;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public class FloorActuatorCreator : ActuatorCreatorBase
    {
        public FloorActuatorCreator(LegacyMapBuilder builder) : base(builder) { }

        public async Task<FloorActuator> GetFloorActuator(IEnumerable<ActuatorItemData> actuators)
        {
            var floorSensors = await Task.WhenAll(actuators.Select(CreateSensor));

            var res = new FloorActuator(floorSensors);
            res.Renderer = builder.RendererSource.GetFloorActuatorRenderer(res);
            return res;
        }

        protected async Task SetupFloorInitializer(SensorInitializer<IActuatorX> initializer, ActuatorItemData data)
        {
            if (data.Decoration > 0)
            {
                var texture = builder.FloorTextures[data.Decoration - 1];
                var decoration = new DecorationItem();
                decoration.Renderer = builder.RendererSource.GetDecorationRenderer(decoration, texture);
                initializer.Graphics = decoration;
            }
            await SetupInitializer(initializer, data);
        }


        private async Task<FloorSensor> CreateSensor(ActuatorItemData arg)
        {

            SensorInitializer<IActuatorX> initializer = new SensorInitializer<IActuatorX>();
            await SetupFloorInitializer(initializer, arg);
            switch (arg.ActuatorType)
            {
                case 5:
                case 0:
                    throw new InvalidOperationException();
                case 1:
                    return new FloorSensorC01(initializer);
                case 2:
                    return new FloorSensorC02(initializer);
                case 3:
                    var directionInitializer = new DirectionIntializer { Direction = GetDirection(arg.Data) };
                    await SetupFloorInitializer(directionInitializer, arg);
                    return new FloorSensorC03(directionInitializer);
                case 4:
                case 8:
                    var constrainSensorInitalizer = new ItemConstrainSensorInitalizer<IActuatorX> { Data = builder.GetItemFactory(arg.Data) };
                    await SetupFloorInitializer(constrainSensorInitalizer, arg);
                    if (arg.ActuatorType == 4)
                        return new FloorSensorC04(constrainSensorInitalizer);
                    else
                        return new FloorSensorC08(constrainSensorInitalizer);
                case 6:
                    return new FloorSensorC06(initializer);
                case 7:
                    return new FloorSensorC07(initializer);
                default:
                    throw new InvalidOperationException();
            }
        }

        private MapDirection? GetDirection(int data)
        {
            switch (data)
            {
                case 0:
                    return null;
                case 1:
                    return MapDirection.North;  
                case 2:
                    return MapDirection.East;  
                case 3:
                    return MapDirection.South;  
                case 4:
                    return MapDirection.West;
                default:
                    throw new InvalidOperationException();
            }
        }
    }


    public class ActuatorCreatorBase
    {
        protected readonly LegacyMapBuilder builder;

        public ActuatorCreatorBase(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        protected async Task<TInitializer> SetupInitializer<TInitializer>(TInitializer initializer, ActuatorItemData data) where TInitializer : SensorInitializerX
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
            initializer.TimeDelay = 1000 / 6 * data.ActionDelay;

            initializer.Specifer = remote?.Position.Direction.ToMapDirection() ?? MapDirection.North; 
            
            var tileResult = await builder.GetTargetTile(remote?.Position.Position.ToAbsolutePosition(builder.CurrentMap), initializer.Specifer);
            initializer.TargetTile = tileResult?.Item1;
            if (tileResult != null) //invertDirection
                initializer.Specifer = tileResult.Item2; 


            return initializer;
        }
    }
}