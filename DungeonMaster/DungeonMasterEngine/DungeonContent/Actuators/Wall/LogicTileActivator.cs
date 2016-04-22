using System;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class LogicTileActivator : TileContentActivator
    {
        public int BitIndex { get; }
        public ActionState BitAction { get; }

        public LogicTileActivator( ActionStateX action)
        {
            BitIndex = action.Specifer;
            BitAction = action.Action;
        }

        public override void ActivateContent(LogicTile t)
        {
            foreach (var gate in t.Gates)
            {
                switch (BitAction)
                {
                    case ActionState.Clear:
                        gate[BitIndex] = false;
                        break;
                    case ActionState.Set:
                        gate[BitIndex] = true;
                        break;
                    case ActionState.Toggle:
                        gate[BitIndex] ^= true;
                        break;
                    case ActionState.Hold:
                        throw new InvalidOperationException();
                }
            }

            foreach (var counter in t.Counters)
            {
                switch (BitAction)
                {
                    case ActionState.Set:
                        counter.Increase();
                        break;
                    case ActionState.Clear:
                    case ActionState.Toggle:
                        counter.Decrease();
                        break;
                    case ActionState.Hold:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}