using log4net;
using System;

namespace ILBLI.Core
{
    public class LogHelper
    {
        public static readonly ILog log = LogManager.GetLogger("sysLog");


        public static void Info(object message)
        {
            log.Info(message);
        }

        public static void Warn(object message)
        {
            log.Warn(message);
        }
        public static void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public static void Debug(object message)
        {
            log.Debug(message);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }


    }
}
