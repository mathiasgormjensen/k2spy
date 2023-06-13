using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class AsyncEventHandlerExtensionMethods
    {
        public static async Task InvokeAsync(this AsyncEventHandler that, object sender, EventArgs e)
        {
            foreach (AsyncEventHandler asyncEventHandler in that.GetInvocationListOrEmpty().Cast<AsyncEventHandler>())
                await asyncEventHandler.Invoke(sender, e);
        }

        public static async Task InvokeAsync<TEventArgs>(this AsyncEventHandler<TEventArgs> that, object sender, TEventArgs e)
        {
            foreach (AsyncEventHandler<TEventArgs> asyncEventHandler in that.GetInvocationListOrEmpty().Cast<AsyncEventHandler<TEventArgs>>())
                await asyncEventHandler.Invoke(sender, e);
        }
    }
}
