using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.WallSensors
{
    public abstract class WallSensor : SensorX
    {
        public WallSensor(SensorInitializerX initializer) : base(initializer) {}


        protected abstract bool Interact(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor);

        public virtual bool TryTrigger(ILeader theron, WallActuator actuator, bool isLast)
        {
            bool notTriggered;

            if (Disabled)
                return false;

            if (!Interact(theron, actuator, isLast, out notTriggered))
                return false;

            var usedEffect = Effect;
            if (usedEffect == SensorEffect.C03_EFFECT_HOLD)
            {
                usedEffect = notTriggered ? SensorEffect.C01_EFFECT_CLEAR : SensorEffect.C00_EFFECT_SET;
                notTriggered = false;
            }

            if (!notTriggered)
            {
                if (Audible)
                {
                    //TODO play audio 
                    //F064_aadz_SOUND_RequestPlay_COPYPROTECTIOND(C01_SOUND_SWITCH, G306_i_PartyMapX, G307_i_PartyMapY, C01_MODE_PLAY_IF_PRIORITIZED);
                }

                if (OnceOnly)
                    Disabled = true;

                F272_xxxx_SENSOR_TriggerEffect(theron, actuator, usedEffect);
            }

            return !notTriggered;
        }
    }
}