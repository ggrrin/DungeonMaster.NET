using System;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class LogicTileActivator : TileContentActivator
    {
        public int BitIndex { get; }
        public Actuators.ActionState BitAction { get; }

        public LogicTileActivator( ActionStateX action)
        {
            BitIndex = action.Specifer;
            BitAction = action.Action;
        }

        //public override void ActivateContent(LogicTile t)
        //{
        //    foreach (var gate in t.Gates)
        //    {
        //        switch (BitAction)
        //        {
        //            case Actuators.ActionState.Clear:
        //                gate[BitIndex] = false;
        //                break;
        //            case Actuators.ActionState.Set:
        //                gate[BitIndex] = true;
        //                break;
        //            case Actuators.ActionState.Toggle:
        //                gate[BitIndex] ^= true;
        //                break;
        //            case Actuators.ActionState.Hold:
        //                throw new InvalidOperationException();
        //        }
        //    }

        //    foreach (var counter in t.Counters)
        //    {
        //        switch (BitAction)
        //        {
        //            case Actuators.ActionState.Set:
        //                counter.Increase();
        //                break;
        //            case Actuators.ActionState.Clear:
        //            case Actuators.ActionState.Toggle:
        //                counter.Decrease();
        //                break;
        //            case Actuators.ActionState.Hold:
        //                throw new InvalidOperationException();
        //        }
        //    }
        //}
    }
}