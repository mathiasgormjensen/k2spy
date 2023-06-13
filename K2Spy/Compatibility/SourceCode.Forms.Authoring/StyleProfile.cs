using K2Spy.Compatibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Forms.Authoring
{
#if !StyleProfile
    public class StyleProfile
    {
        public void FromJson(string json)
        {
            throw new CompatibilityImplementationException();
        }

        public string ToXml()
        {
            throw new CompatibilityImplementationException();
        }
    }
#endif
}