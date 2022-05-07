using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Services
{
    public class LogService
    {
        public static LogFactory GetLogFactory()
        {
            return NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config");
        }
    }
}
