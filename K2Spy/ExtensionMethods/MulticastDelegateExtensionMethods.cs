using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class MulticastDelegateExtensionMethods
    {
        public static Delegate[] GetInvocationListOrEmpty(this MulticastDelegate that)
        {
            return (that?.GetInvocationList()) ?? new Delegate[0];
        }

        public static void ForEach(this MulticastDelegate that, Action<EventHandler> callback)
        {
            that.ForEach<EventHandler>(callback);
        }

        public static void ForEach<TEventHandler>(this MulticastDelegate that, Action<TEventHandler> callback)
        {
            foreach (TEventHandler eventHandler in that.GetInvocationListOrEmpty().Cast<TEventHandler>())
                callback(eventHandler);
        }
    }
}
