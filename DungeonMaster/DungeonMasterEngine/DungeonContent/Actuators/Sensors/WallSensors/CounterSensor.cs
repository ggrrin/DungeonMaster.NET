using System;
using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    public class CounterSensor : SensorX
    {
        private int count;

        public int Count
        {
            get { return count; }
            private set
            {
                if (Count > 0)
                {
                    //if (P524_ps_Event->C.A.Effect == C00_EFFECT_SET)
                    //{
                    //    L0637_ui_SensorData++;
                    //}
                    //else
                    //{
                    //    L0637_ui_SensorData--;
                    //}
                    count = value;

                    if (Effect == SensorEffect.C03_EFFECT_HOLD)
                    {
                        bool A0636_B_TriggerSetEffect = ((count == 0) != RevertEffect);
                        TriggerEffect(null, null, A0636_B_TriggerSetEffect ? SensorEffect.C00_EFFECT_SET : SensorEffect.C01_EFFECT_CLEAR);
                    }
                    else
                    {
                        if (Count == 0)
                        {
                            TriggerEffect(null, null, Effect);
                        }
                    }
                }
            }
        }


        private void Increase() => Count++;

        private void Decrease() => Count--;

        public override void AcceptMessage(Message message)
        {
            switch (message.Action)
            {
                case MessageAction.Set:
                    Increase();
                    break;
                case MessageAction.Clear:
                    Decrease();
                    break;
                case MessageAction.Toggle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public CounterSensor(CounterIntializer initializer) : base(initializer)
        {
            count = initializer.Count;
        }

        public override IActuatorX GraphicsBase => null;
    }
}