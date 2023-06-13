using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class StopWatch : IDisposable
    {
        private DateTime m_Begin = DateTime.Now;
        private string m_Name;

        public StopWatch(string name = "")
        {
            this.m_Name = name;
            if (!string.IsNullOrEmpty(name))
                Serilog.Log.Debug("// " + name);
        }

        public static StopWatch CurrentMethod([CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int? lineNumber = null)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(filePath))
            {
                builder.Append(System.IO.Path.GetFileName(filePath));
                if (lineNumber > 0)
                    builder.Append($"#{lineNumber}");
                builder.Append(", ");
            }
            builder.Append(methodName);
            return new StopWatch(builder.ToString());
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(this.m_Name))
                Serilog.Log.Debug("## " + this.m_Name + ": " + DateTime.Now.Subtract(this.m_Begin));
            else
                Serilog.Log.Debug(DateTime.Now.Subtract(this.m_Begin).ToString());
        }
    }
}
