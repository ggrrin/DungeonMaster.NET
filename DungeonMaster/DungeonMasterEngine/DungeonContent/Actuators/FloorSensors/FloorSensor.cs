using System;

namespace DungeonMasterEngine.DungeonContent.Actuators.FloorSensors
{
    public class FloorSensor : FloorSensorX 
    {
        public FloorSensor(SensorInitializer<IActuatorX> initializer) : base(initializer)
        {
            Graphics = initializer.Graphics;
        }

        public IActuatorX Graphics { get; }
        public override IActuatorX GraphicsBase => Graphics;

        protected override bool X(ref bool triggerSensor, object obj, bool entering, bool alreadyContainsParty, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            throw new NotImplementedException();
        }
    }
}