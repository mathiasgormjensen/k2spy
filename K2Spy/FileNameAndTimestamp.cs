using Newtonsoft.Json.Linq;
using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class FileNameAndTimestamp
    {
        public FileNameAndTimestamp(string fileName, DateTime timestamp)
        {
            this.FileName = fileName;
            this.Timestamp = timestamp;
        }
        public FileNameAndTimestamp(Guid fileName, DateTime timestamp)
            : this(fileName.ToString("N"), timestamp)
        {
        }

        public string FileName { get; private set; }

        public DateTime Timestamp { get; private set; }

        public DateTime TimestampUtc
        {
            get { return this.Timestamp.ToUniversalTime(); }
        }
    }
}