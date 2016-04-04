using Microsoft.Xna.Framework;
using System;
using DungeonMasterEngine.DungeonContent.Items.Actuators;
using System.Linq;
using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class LogicTile : Tile
    {
        public override bool IsAccessible => false;

        public IEnumerable<LogicGate> Gates { get; set; }

        public LogicTile(IEnumerable<LogicGate> gates) : base(Vector3.Zero)
        {
            Gates = gates.ToArray();
        }
 
        public override void ExecuteContentActivator(ITileContentActivator contentActivator)
        {
            contentActivator.ActivateContent(this);
        }
    }

    public class LogicGate
    {
        public bool CurrentBit0 { get; set; }
        public bool CurrentBit1 { get; set; }
        public bool CurrentBit2 { get; set; }
        public bool CurrentBit3 { get; set; }

        public bool ReferenceBit0 { get; }
        public bool ReferenceBit1 { get; }
        public bool ReferenceBit2 { get; }
        public bool ReferenceBit3 { get; }
        
        public ActionState Action { get; }
        public Tile TargetTile { get; }

        public LogicGate(Tile targetTile, ActionState action, bool refBit0, bool refBit1, bool refBit2, bool refBit3)
        {
            Action = action;
            TargetTile = targetTile;
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

                TriggerAction();
            }
        }

        private void TriggerAction()
        {
            if (CurrentBit0 == ReferenceBit0 && CurrentBit1 == ReferenceBit1 && CurrentBit2 == ReferenceBit2 && CurrentBit3 == ReferenceBit3)
            {
                switch (Action)
                {
                    case ActionState.Clear:
                        TargetTile.DeactivateTileContent();
                        break;
                    case ActionState.Set:
                        TargetTile.ActivateTileContent();
                        break;
                    case ActionState.Toggle:
                        if (TargetTile.ContentActivated)
                            TargetTile.DeactivateTileContent();
                        else
                            TargetTile.ActivateTileContent();
                        break;
                    case ActionState.Hold:
                        TargetTile.ActivateTileContent();
                        //TODO
                        break;
                }
            }
        }
    }
}