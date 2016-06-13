using System;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public abstract class Sensor<TActuatorX> : WallSensor, IMessageAcceptor<Message> where TActuatorX : IActuatorX
    {

        public TActuatorX Graphics { get; }
        public override IActuatorX GraphicsBase => Graphics;

        public Sensor(SensorInitializer<TActuatorX> initializer) : base(initializer)
        {
            
            Graphics = initializer.Graphics;
        }
    }

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

    public abstract class SensorX
    {
        public SensorEffect Effect { get; }
        //local target
        public bool ExperienceGain { get; }
        public bool Rotate { get; }

        //remote target
        public MapDirection Specifer { get; }
        public Tile TargetTile { get; }

        public int TimeDelay { get; }
        public bool LocalEffect { get; }
        public bool RevertEffect { get; }
        public bool OnceOnly { get; }
        public bool Audible { get; }

        public bool Disabled { get; protected set; }



        public abstract IActuatorX GraphicsBase { get; }

        protected SensorX(SensorInitializerX initializer)
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


            if(!LocalEffect && TargetTile == null)
                throw new ArgumentException("tile shouldnt be null");
        }




        protected async void F272_xxxx_SENSOR_TriggerEffect(ILeader theron, ActuatorX actuator, SensorEffect effect)
        {
            if (LocalEffect)
            {
                F270_xxxx_SENSOR_TriggerLocalEffect(theron, actuator);
            }
            else
            {
                MessageAction action;
                switch (effect)
                {
                    case SensorEffect.C00_EFFECT_SET:
                        action = MessageAction.Set;
                        break;
                    case SensorEffect.C01_EFFECT_CLEAR:
                        action = MessageAction.Clear;
                        break;
                    case SensorEffect.C02_EFFECT_TOGGLE:
                        action = MessageAction.Toggle;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
                }

                //TODO send B,A,TargetCell only to wall tiles, floor tiles has set this flag NORTHWEST
                //TODO time delay await Task.Delay(A.Value);
                await Task.Delay(TimeDelay);
                TargetTile.AcceptMessage(new Message(action, Specifer));
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
            if (theron == null)
                return;

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

        public virtual void AcceptMessage(Message message) {}
    }
}