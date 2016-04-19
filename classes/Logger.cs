using log4net;
using System;
using System.Linq;

namespace TERA_Tweaker.classes
{
    //public enum LogLevel
    //{
    //    Debug,
    //    Info,
    //    Warn,
    //    Error,
    //    Fatal
    //}

    public class Logger<T>
    {
        private static readonly ILog log;

        static Logger()
        {
            log = LogManager.GetLogger(GetLoggerName<T>());
        }

        private static string GetLoggerName<TL>()
        {
            if (typeof(TL).Equals(typeof(Logger)))
            {
                return "TERA-Tweaker";
            }
            else
                return typeof(TL).FullName;
        }

        public static void Error(string message, Exception e, params object[] args)
        {
            if (log.IsErrorEnabled)
            {
                string logMsg = GetResolvedMessage(message, args);
                log.Error(message, e);
            }
        }

        public static void Error(string message, params object[] args)
        {
            if (log.IsErrorEnabled)
            {
                string logMsg = GetResolvedMessage(message, args);
                log.Error(message);
            }
        }

        public static void Debug(string message, params object[] args)
        {
            if (log.IsDebugEnabled)
            {
                string logMsg = GetResolvedMessage(message, args);
                log.Debug(message);
            }
        }

        public static void Warn(string message, Exception e, params object[] args)
        {
            if (log.IsWarnEnabled)
            {
                string logMsg = GetResolvedMessage(message, args);
                log.Warn(logMsg, e);
            }
        }

        public static void Warn(string message, params object[] args)
        {
            if (log.IsWarnEnabled)
            {
                string logMsg = GetResolvedMessage(message, args);
                log.Warn(logMsg);
            }
        }

        public static void Info(string message, params object[] args)
        {
            if (log.IsInfoEnabled)
            {
                string logMsg = GetResolvedMessage(message, args);
                log.Info(logMsg);
            }
        }

        public static void Fatal(string message, params object[] args)
        {
            if (log.IsFatalEnabled)
            {
                string logMsg = GetResolvedMessage(message, args);
                log.Fatal(logMsg);
            }
        }

        public static void Fatal(string message, Exception e, params object[] args)
        {
            if (log.IsFatalEnabled)
            {
                string logMsg = GetResolvedMessage(message, args);
                log.Fatal(logMsg, e);
            }
        }

        private static string GetResolvedMessage(string message, params object[] args)
        {
            string result;
            if (args != null && args.Count() > 0)
                result = string.Format(message, args);
            else
                result = message;

            return result;
        }
    }

    public class Logger : Logger<Logger>
    {

    }
}
