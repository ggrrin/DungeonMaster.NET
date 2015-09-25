using System;
using System.Collections.Generic;
using System.Threading;

namespace DungeonMasterEngine
{
    internal class GameSynchronizationContext : SynchronizationContext
    {
        private Queue<Tuple<SendOrPostCallback, object>> executionQueue;

        public GameSynchronizationContext(Queue<Tuple<SendOrPostCallback,object>> executionQueue)
        {
            this.executionQueue = executionQueue;
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            executionQueue.Enqueue(new Tuple<SendOrPostCallback, object>(d, state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            d(state);
        }

    }
}