using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Jobs
{
    public abstract class BaseThread
    {
        private bool canStop;
        protected volatile Thread Instance;
        protected TimeSpan timeToRun;

        public async void Start()
        {
            canStop = false;

            while (!canStop)
            {
                DateTime now = DateTime.Now;
                int miliseconds = (int)(now.TimeOfDay.TotalMilliseconds > timeToRun.TotalMilliseconds ?
                    (new TimeSpan(24,0,0).TotalMilliseconds - now.TimeOfDay.TotalMilliseconds) + timeToRun.TotalMilliseconds :
                    timeToRun.TotalMilliseconds - now.TimeOfDay.TotalMilliseconds);

               Thread.Sleep(miliseconds);
                Run();
            }
        }

        public void Stop()
        {
            canStop = true;
        }

        public abstract void Init(TimeSpan time);
        public abstract void Run();
    }
}