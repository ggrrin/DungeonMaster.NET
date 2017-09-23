using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.FloorSensors
{
    public abstract class FloorSensor : SensorX 
    {
        public FloorSensor(SensorInitializer<IActuatorX> initializer) : base(initializer)
        {
            Graphics = initializer.Graphics;
        }

        public IActuatorX Graphics { get; }
        public override IActuatorX GraphicsBase => Graphics;

        protected abstract bool TryInteract(ref bool triggerSensor, object obj, bool entering, bool alreadyContainsParty, bool alreadyContainsCreature, bool containsThingOfSameType, bool containThingOfDifferentType, bool alreadyContainsItem);

        public bool TryTrigger(ActuatorX actuator, object obj, bool entering, bool alreadyContainsParty, bool alreadyContainsCreature, bool alreadyContainsGrabable,
            bool alreadyContainsThingOfSameType, bool alreadyContainsThingOfDifferentType)
        {
            if (Disabled)
                return false;

            bool L0768_B_TriggerSensor = entering;

            if (!TryInteract(ref L0768_B_TriggerSensor, obj, entering, alreadyContainsParty, alreadyContainsCreature, alreadyContainsThingOfSameType, alreadyContainsThingOfDifferentType, alreadyContainsGrabable))
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
            TriggerEffect(obj as ILeader, actuator, finalEffect);
            return true;
        }

    }
}