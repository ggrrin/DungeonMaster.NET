using System;
using DungeonMasterEngine.DungeonContent.Actuators.WallSensors;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class LogicGateSensor : SensorX
    {
        public bool CurrentBit0 { get; set; }
        public bool CurrentBit1 { get; set; }
        public bool CurrentBit2 { get; set; }
        public bool CurrentBit3 { get; set; }

        public bool ReferenceBit0 { get; }
        public bool ReferenceBit1 { get; }
        public bool ReferenceBit2 { get; }
        public bool ReferenceBit3 { get; }


        public LogicGateSensor(LogicGateInitializer initializer) : base(initializer)
        {
            ReferenceBit0 = initializer.RefBit0;
            ReferenceBit1 = initializer.RefBit1;
            ReferenceBit2 = initializer.RefBit2;
            ReferenceBit3 = initializer.RefBit3;
        }

        public override void AcceptMessage(Message message)
        {
             

            switch (message.Action)
            {
                case MessageAction.Set:
                    this[message.Specifier.Index] = true;
                    break;
                case MessageAction.Clear:
                    this[message.Specifier.Index] = false;
                    break;
                case MessageAction.Toggle:
                    this[message.Specifier.Index] ^= true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool this[int bitIndex]
        {
            get
            {
                switch (bitIndex)
                {
                    case 0:
                        return CurrentBit0;
                    case 1:
                        return CurrentBit1;
                    case 2:
                        return CurrentBit2;
                    case 3:
                        return CurrentBit3;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            set
            {
                switch (bitIndex)
                {
                    case 0:
                        CurrentBit0 = value;
                        break;
                    case 1:
                        CurrentBit1 = value;
                        break;
                    case 2:
                        CurrentBit2 = value;
                        break;
                    case 3:
                        CurrentBit3 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                bool some = CurrentBit0 == ReferenceBit0 && CurrentBit1 == ReferenceBit1 && CurrentBit2 == ReferenceBit2 && CurrentBit3 == ReferenceBit3;
                bool A0636_B_TriggerSetEffect = some;//TODO F161_sz...// != RevertEffect;
                if (Effect == SensorEffect.C03_EFFECT_HOLD)
                {
                    F272_xxxx_SENSOR_TriggerEffect(null, null, A0636_B_TriggerSetEffect ? SensorEffect.C00_EFFECT_SET : SensorEffect.C01_EFFECT_CLEAR);
                }
                else
                {
                    if (A0636_B_TriggerSetEffect)
                    {
                        F272_xxxx_SENSOR_TriggerEffect(null, null, Effect);
                    }
                }
            }
        }

        public override IActuatorX GraphicsBase => null;
    }
}
