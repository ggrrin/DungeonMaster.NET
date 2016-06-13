using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall.FloorSensors
{
    public abstract class FloorSensorX : SensorX
    {


        protected abstract bool X(ref bool triggerSensor, object obj, bool entering, bool alreadyContainsParty, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem);

        public bool TryInteract(ActuatorX actuator, object obj, bool entering, bool alreadyContainsParty, bool alreadyContainsCreature, bool alreadyContainsGrabable,
            bool alreadyContainsThingOfSameType, bool alreadyContainsThingOfDifferentType)
        {
            if (Disabled)
                return false;

            bool L0768_B_TriggerSensor = entering;

            if (!X(ref L0768_B_TriggerSensor, obj, entering, alreadyContainsParty, alreadyContainsCreature, alreadyContainsThingOfSameType, alreadyContainsThingOfDifferentType, alreadyContainsGrabable))
                return false;


            L0768_B_TriggerSensor ^= RevertEffect;
            var finalEffect = Effect;
            if (Effect == SensorEffect.C03_EFFECT_HOLD)
            {
                finalEffect= L0768_B_TriggerSensor ? SensorEffect.C00_EFFECT_SET : SensorEffect.C01_EFFECT_CLEAR;
            }
            else
            {
                if (!L0768_B_TriggerSensor)
                {
                    return false;
                }
            }

            if (Audible)
            {
                //TODO sound
                //F064_aadz_SOUND_RequestPlay_COPYPROTECTIOND(C01_SOUND_SWITCH, P588_ui_MapX, P589_ui_MapY, C01_MODE_PLAY_IF_PRIORITIZED);
            }
            F272_xxxx_SENSOR_TriggerEffect(obj as ILeader, actuator, finalEffect);
            return true;
        }

        public FloorSensorX(SensorInitializerX initializer) : base(initializer) { }

    }
}