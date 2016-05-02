using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders
{
    public abstract class ActuatorFactoryBase : FactoryBase<ActuatorState, ActuatorItemData, LegacyMapBuilder, Actuator> { }
}