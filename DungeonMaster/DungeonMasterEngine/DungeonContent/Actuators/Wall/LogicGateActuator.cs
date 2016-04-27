using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class LogicGate : RemoteActuator
    {
        public bool CurrentBit0 { get; set; }
        public bool CurrentBit1 { get; set; }
        public bool CurrentBit2 { get; set; }
        public bool CurrentBit3 { get; set; }

        public bool ReferenceBit0 { get; }
        public bool ReferenceBit1 { get; }
        public bool ReferenceBit2 { get; }
        public bool ReferenceBit3 { get; }

        public override ActionStateX TargetAction { get; }

        public LogicGate(Tile targetTile, ActionStateX action, Vector3 position, bool refBit0, bool refBit1, bool refBit2, bool refBit3) : base(targetTile, position)
        {
            Activated = false;
            TargetAction = action;
            ReferenceBit0 = refBit0;
            ReferenceBit1 = refBit1;
            ReferenceBit2 = refBit2;
            ReferenceBit3 = refBit3;
        }

        public bool this[int bitIndex]
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

                    if (CurrentBit0 == ReferenceBit0 && CurrentBit1 == ReferenceBit1 && CurrentBit2 == ReferenceBit2 && CurrentBit3 == ReferenceBit3)
                    {
                        Activated = true;
                        SendMessageAsync();
                    }

                if (Activated)
                {
                    if (!(CurrentBit0 == ReferenceBit0 && CurrentBit1 == ReferenceBit1 && CurrentBit2 == ReferenceBit2 && CurrentBit3 == ReferenceBit3))
                    {
                        Activated = false;
                        SendMessageAsync();
                    }
                }
            }
        }

    }
}
