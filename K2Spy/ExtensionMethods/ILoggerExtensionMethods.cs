using SourceCode.Forms.Authoring.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class ILoggerExtensionMethods
    {
        public static void CurrentMethod(this Serilog.ILogger that, (string name, object value) argument, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int? lineNumber = null)
        {
            ILoggerExtensionMethods.CurrentMethod(that, new (string, object)[] { argument }, methodName, filePath, lineNumber);
        }

        public static void CurrentMethod(this Serilog.ILogger that, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int? lineNumber = null)
        {
            ILoggerExtensionMethods.CurrentMethod(that, null, methodName, filePath, lineNumber);
        }

        public static void CurrentMethod(this Serilog.ILogger that, (string name, object value)[] arguments, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int? lineNumber = null)
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
            if (arguments?.Length > 0)
            {
                foreach ((string, object) argument in arguments)
                {
                    builder.Append($", {argument.Item1}={argument.Item2}");
                }
            }
            that.Write(Serilog.Events.LogEventLevel.Debug, builder.ToString());
        }
    }
}
