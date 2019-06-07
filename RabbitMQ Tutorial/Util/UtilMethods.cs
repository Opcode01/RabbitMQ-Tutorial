using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class UtilMethods
    {
        public static string ParseMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
    public static class LogTypes
    {
        public const string Info = "info";
        public const string Warning = "warning";
        public const string Error = "error";
    }
}
