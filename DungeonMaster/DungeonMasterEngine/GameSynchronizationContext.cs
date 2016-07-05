using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DungeonMasterEngine
{
    internal class GameSynchronizationContext : SynchronizationContext
    {
        private readonly Queue<Tuple<SendOrPostCallback, object>> executionQueue;

        public GameSynchronizationContext(Queue<Tuple<SendOrPostCallback, object>> executionQueue)
        {
            this.executionQueue = executionQueue;
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            //Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
            var t = Tuple.Create(d, state);
            lock (executionQueue)
                executionQueue.Enqueue(t);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            Post(d, state);
        }
    }
}