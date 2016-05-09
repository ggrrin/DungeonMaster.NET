using System.Linq;
using DungeonMasterEngine.Builders.FloorActuatorFactories;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public class FloorActuatorCreator
    {
        private readonly LegacyMapBuilder builder;
        private readonly Parser<ActuatorState, ActuatorItemData, LegacyMapBuilder, Actuator> parser;


        public FloorActuatorCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
            parser = new Parser<ActuatorState, ActuatorItemData, LegacyMapBuilder, Actuator>(new ActuatorFactoryBase[]
            {
                //TODO add factories
                new FloorDirectionFactory(), 
                new FloorCreatureFactory(), 
                new ItemFactory(), 
                new PartyPossesionFactory(), 
                new TPCFactory(), 
                new TPCIFactory(), 
                new MultiFloorDirectionFactory(), 
            });
        }

        public void CreateSetupActuators(Tile currentTile)
        {
            var actuators = builder.CurrentMap.GetTileData(currentTile.GridPosition).Actuators;
            if (actuators.Any())
            {
                var factory = parser.TryMatchFactory(actuators, false);
                if (factory != null)
                {
                    currentTile.SubItems.Add(factory.CreateItem(builder, currentTile, actuators));
                }
                else
                {
                    if (actuators.All(x => x.ActuatorType != 5 && x.ActuatorType != 6))
                    {
                        foreach (var i in actuators)
                        {
                            Point? absolutePosition = null;
                            if (i.ActionLocation is RemoteTarget)
                                absolutePosition = ((RemoteTarget)i.ActionLocation).Position.Position.ToAbsolutePosition(builder.CurrentMap);

                            currentTile.SubItems.Add(new Actuator(builder.GetWallPosition(i.TilePosition, currentTile), $"{absolutePosition} {i.DumpString()}"));
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

    }
}