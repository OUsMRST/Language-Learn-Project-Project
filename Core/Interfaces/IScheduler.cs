using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IScheduler
    {
        /// Schedules the time and parameters of the next repetition and returns the updated card.
        public void Schedule(int assessment, ref Card card);
    }
}
