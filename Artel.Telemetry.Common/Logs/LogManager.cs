using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artel.Telemetry.Common.Logs
{
    public static class LogManager
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(LogManager));
        public static void Info(object message)
        {
            log.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }
    }
}
