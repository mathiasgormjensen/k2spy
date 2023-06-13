using K2Spy.Compatibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Forms.Management
{
#if !StyleProfile
    public class StyleProfileInfo
    {
        public Guid Guid
        {
            get { throw new CompatibilityImplementationException(); }
        }

        public DateTime ModifiedDate
        {
            get { throw new CompatibilityImplementationException(); }
        }

        public DateTime CreatedDate
        {
            get { throw new CompatibilityImplementationException(); }
        }

        public string CreatedBy
        {
            get { throw new CompatibilityImplementationException(); }
        }

        public string DisplayName
        {
            get { throw new CompatibilityImplementationException(); }
        }

        public string ModifiedBy
        {
            get { throw new CompatibilityImplementationException(); }
        }

        public string CheckedOutBy
        {
            get { throw new CompatibilityImplementationException(); }
        }

        public string CategoryPath
        {
            get { throw new CompatibilityImplementationException(); }
        }

        public bool IsCheckedOut
        {
            get { throw new CompatibilityImplementationException(); }
        }
    }
#endif
}