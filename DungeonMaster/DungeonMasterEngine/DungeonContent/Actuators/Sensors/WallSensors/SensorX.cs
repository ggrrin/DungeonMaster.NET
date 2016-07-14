using System;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    public abstract class SensorX
    {
        public SensorEffect Effect { get; }
        //local target
        public bool ExperienceGain { get; }
        public bool Rotate { get; }

        //remote target
        public MapDirection Specifier { get; }
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
            Specifier = initializer.Specifer;
            TargetTile = initializer.TargetTile;
            TimeDelay = initializer.TimeDelay;
            LocalEffect = initializer.LocalEffect;
            RevertEffect = initializer.RevertEffect;
            OnceOnly = initializer.OnceOnly;
            Audible = initializer.Audible;


            if(!LocalEffect && TargetTile == null)
                throw new ArgumentException("tile shouldnt be null");
        }




        //F272_xxxx_SENSOR_TriggerEffect
        protected async void TriggerEffect(ILeader theron, ActuatorX actuator, SensorEffect effect)
        {
            if (LocalEffect)
            {
                TriggerLocalEffect(theron, actuator);
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
                TargetTile.AcceptMessageBase(new Message(action, Specifier));
            }
        }

        //F270_xxxx_SENSOR_TriggerLocalEffect
        protected void TriggerLocalEffect(ILeader theron, ActuatorX actuator, bool? rotate = null)
        {
            if (ExperienceGain)
            {
                //TODO on floor tiles add skill to all party members
                AddExperience(theron, SkillFactory<StealSkill>.Instance, 300, true /*P574_i_SensorLocalEffectCell != CM1_CELL_ANY*/);
            }
            else if (rotate ?? Rotate)
            {
                actuator.Rotate = Rotate;
            }
        }

         //F269_xxxx_SENSOR_AddSkillExperience
        void AddExperience(ILeader theron, ISkillFactory skill, int exprience, bool P570_B_LeaderOnly)
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