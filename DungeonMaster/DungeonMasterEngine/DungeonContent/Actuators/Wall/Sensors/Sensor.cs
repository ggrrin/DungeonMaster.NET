using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public abstract class Sensor : IMessageAcceptor<Message>
    {
        public SensorEffect Effect { get; }
        //local target
        public bool ExperienceGain { get; }
        public bool Rotate { get; }

        //remote target
        public EarthSides Specifer { get; }
        public Tile TargetTile { get; }

        public int TimeDelay { get; }
        public bool LocalEffect { get; }
        public bool RevertEffect { get; }
        public bool OnceOnly { get; }
        public bool Audible { get; }

        public bool Disabled { get; protected set; }

        public IActuatorX Graphics { get; }

        protected Sensor(SensorInitializer initializer)
        {
            Effect = initializer.Effect;
            ExperienceGain = initializer.ExperienceGain;
            Rotate = initializer.Rotate;
            Specifer = initializer.Specifer;
            TargetTile = initializer.TargetTile;
            TimeDelay = initializer.TimeDelay;
            LocalEffect = initializer.LocalEffect;
            RevertEffect = initializer.RevertEffect;
            OnceOnly = initializer.OnceOnly;
            Audible = initializer.Audible;
            Graphics = initializer.Graphics;
        }

        protected abstract bool Interact(ILeader theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor);

        public virtual bool TryTrigger(ILeader theron, ActuatorX actuator, bool isLast)
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


        async void F272_xxxx_SENSOR_TriggerEffect(ILeader theron, ActuatorX actuator, SensorEffect P576_i_Effect)
        {
            if (LocalEffect)
            {
                F270_xxxx_SENSOR_TriggerLocalEffect(theron, actuator);
            }
            else
            {
                //TODO send B,A,TargetCell only to wall tiles, floor tiles has set this flag NORTHWEST
                //TODO time delay await Task.Delay(A.Value);
                await Task.Delay(TimeDelay);
                //TODO send message
            }
        }

        protected void F270_xxxx_SENSOR_TriggerLocalEffect(ILeader theron, ActuatorX actuator, bool? rotate = null)
        {
            if (ExperienceGain)
            {
                //TODO on floor tiles add skill to all party members
                F269_xxxx_SENSOR_AddSkillExperience(theron, SkillFactory<StealSkill>.Instance, 300, true /*P574_i_SensorLocalEffectCell != CM1_CELL_ANY*/);
            }
            else if (rotate ?? Rotate)
            {
                actuator.Rotate = Rotate;
            }
        }


        void F269_xxxx_SENSOR_AddSkillExperience(ILeader theron, ISkillFactory skill, int exprience, bool P570_B_LeaderOnly)
        {
            if (P570_B_LeaderOnly)
            {
                theron.Leader.GetSkill(skill).AddExperience(exprience);
                return;
            }
            exprience /= theron.PartyGroup.Count;

            foreach (var champion in theron.PartyGroup)
            {
                //TODO champion not/alive
                champion.GetSkill(skill).AddExperience(exprience);
            }
        }

        public virtual void AcceptMessage(Message message)
        { }
    }
}