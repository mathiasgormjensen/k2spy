using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.Extensions.ExecuteSmartObject
{
    public class ExecuteSmartObjectData
    {
        public Guid SmartObjectGuid { get; set; }
        public string SmartObjectSystemName { get; set; }
        public string SmartObjectDisplayName { get; set; }

        public string MethodSystemName { get; set; }
        public string MethodDisplayName { get; set; }

        public InputProperty[] InputProperties { get; set; }
    }
}
