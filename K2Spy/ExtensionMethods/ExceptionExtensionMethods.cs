using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class ExceptionExtensionMethods
    {
        public static string BuildDetailedMessage(this Exception that)
        {
            // TODO implement something intelligent
            return that.ToString();
        }
    }
}
