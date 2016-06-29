using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.FloorSensors
{
    //class FloorSensorC05 : FloorSensor
    //{
    //    protected override bool X(object obj, bool entering, bool containsObj, bool containsGroup, bool containsThingOfSameType, bool containThingOfDifferentType, ref bool triggerSensor)
    //    {
    //        if (!(obj is Theron) || (m34_square_type(l0777_ui_square) != c03_element_stairs))
    //        {
    //            return false;
    //        }
    //        return true;
    //    }
    //}

    class FloorSensorC06 : FloorSensor
    {
        protected override bool TryInteract(ref bool triggerSensor, object obj, bool entering, bool containsObj, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            return false;
        }

        public FloorSensorC06(SensorInitializer<IActuatorX> initializer) : base(initializer) {}
    }
}

