using K2Spy.Compatibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Forms.Management
{
#if !StyleProfile
    public static class FormsManagerExtensionMethods
    {
        public static string GetStyleProfileDefinition(this global::SourceCode.Forms.Management.FormsManager that, Guid ignore)
        {
            throw new CompatibilityImplementationException();
        }

        public static StyleProfilesExplorer GetStyleProfiles(this global::SourceCode.Forms.Management.FormsManager that)
        {
            return new StyleProfilesExplorer();
        }

        public static void CheckInStyleProfile(this global::SourceCode.Forms.Management.FormsManager that, Guid ignore)
        {
            throw new CompatibilityImplementationException();
        }

        public static void UndoStyleProfileCheckOut(this global::SourceCode.Forms.Management.FormsManager that, Guid ignore)
        {
            throw new CompatibilityImplementationException();
        }

        public static void CheckOutStyleProfile(this global::SourceCode.Forms.Management.FormsManager that, Guid ignore)
        {
            throw new CompatibilityImplementationException();
        }
    }
#endif
}
