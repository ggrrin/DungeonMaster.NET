using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public abstract class ActuatorFactoryBase : FactoryBase<ActuatorState, ActuatorItemData, LegacyMapBuilder, Actuator> { }
}