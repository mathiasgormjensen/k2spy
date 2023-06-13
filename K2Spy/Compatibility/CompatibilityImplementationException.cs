using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.Compatibility
{
    public class CompatibilityImplementationException : Exception
    {
        public CompatibilityImplementationException()
            : base("This is part of a compatibility-implementation and should never be used")
        {
        }
    }
}
