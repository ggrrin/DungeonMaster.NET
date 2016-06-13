namespace DungeonMasterEngine.DungeonContent.Actuators.Wall.FloorSensors
{
    class FloorSensorC01 : FloorSensor
    {
        protected override bool X(ref bool triggerSensor, object obj, bool entering, bool alreadyContainsParty, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem)
        {
            if (alreadyContainsParty || alreadyContainsItem || alreadyContainsCreature)
            { /* bug0_30 a floor sensor is not triggered when you put an object on the floor if a levitating creature is present on the same square. the condition to determine if the sensor should be triggered checks if there is a creature on the square but does not check whether the creature is levitating. while it is normal not to trigger the sensor if there is a non levitating creature on the square (because it was already triggered by the creature itself), a levitating creature should not prevent triggering the sensor with an object. */
                return false;
            }
            return true;
        }

        public FloorSensorC01(SensorInitializer<IActuatorX> initializer) : base(initializer) {}
    }
}