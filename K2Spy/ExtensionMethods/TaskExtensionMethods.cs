using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class TaskExtensionMethods
    {
        public static async Task StopWatch(this Task that, string name = "")
        {
            using (new StopWatch(name))
                await that;
        }
        public static async Task<T> StopWatch<T>(this Task<T> that, string name = "")
        {
            using (new StopWatch(name))
                return await that;
        }

        public static void FireAndForget(this System.Threading.Tasks.Task that)
        {
            // do nothing
        }
    }
}