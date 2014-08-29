﻿using ExitGames.Concurrency.Fibers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class Game : IDisposable
    {
        public PoolFiber poolFiber {private set; get;}

        protected MarsPeer marsPeer;
        public Game(MarsPeer peer)
        {
            this.marsPeer = peer;
            this.poolFiber = new PoolFiber();
            this.poolFiber.Start();
        }

        public void EnqueueOperator(params object[] objs)
        {
            poolFiber.Enqueue(() => ExecuteOperation(objs));
        }

        public virtual void ExecuteOperation(params object[] objs)
        {
 
        }

        public void Dispose()
        {
            if (poolFiber != null)
            {
                poolFiber.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}