using System;

namespace BrCms.Framework.Logging
{
    public static class LoggingExtensions
    {
        public static void Debug(this ILogger logger, string name, string message)
        {
            FilteredLog(name, logger, LogLevel.Debug, null, message, null);
        }

        public static void Debug(this ILogger logger, Type type, string message)
        {
            FilteredLog(type, logger, LogLevel.Debug, null, message, null);
        }

        public static void Information(this ILogger logger, string name, string message)
        {
            FilteredLog(name, logger, LogLevel.Information, null, message, null);
        }

        public static void Information(this ILogger logger, Type type, string message)
        {
            FilteredLog(type, logger, LogLevel.Information, null, message, null);
        }

        public static void Warning(this ILogger logger, string name, string message)
        {
            FilteredLog(name, logger, LogLevel.Warning, null, message, null);
        }

        public static void Warning(this ILogger logger, Type type, string message)
        {
            FilteredLog(type, logger, LogLevel.Warning, null, message, null);
        }

        public static void Error(this ILogger logger, string name, string message)
        {
            FilteredLog(name, logger, LogLevel.Error, null, message, null);
        }

        public static void Error(this ILogger logger, Type type, string message)
        {
            FilteredLog(type, logger, LogLevel.Error, null, message, null);
        }

        public static void Fatal(this ILogger logger, string name, string message)
        {
            FilteredLog(name, logger, LogLevel.Fatal, null, message, null);
        }

        public static void Fatal(this ILogger logger, Type type, string message)
        {
            FilteredLog(type, logger, LogLevel.Fatal, null, message, null);
        }

        public static void Debug(this ILogger logger, string name, Exception exception, string message)
        {
            FilteredLog(name, logger, LogLevel.Debug, exception, message, null);
        }

        public static void Debug(this ILogger logger, Type type, Exception exception, string message)
        {
            FilteredLog(type, logger, LogLevel.Debug, exception, message, null);
        }

        public static void Information(this ILogger logger, string name, Exception exception, string message)
        {
            FilteredLog(name, logger, LogLevel.Information, exception, message, null);
        }

        public static void Information(this ILogger logger, Type type, Exception exception, string message)
        {
            FilteredLog(type, logger, LogLevel.Information, exception, message, null);
        }

        public static void Warning(this ILogger logger, string name, Exception exception, string message)
        {
            FilteredLog(name, logger, LogLevel.Warning, exception, message, null);
        }

        public static void Warning(this ILogger logger, Type type, Exception exception, string message)
        {
            FilteredLog(type, logger, LogLevel.Warning, exception, message, null);
        }

        public static void Error(this ILogger logger, string name, Exception exception, string message)
        {
            FilteredLog(name, logger, LogLevel.Error, exception, message, null);
        }

        public static void Error(this ILogger logger, Type type, Exception exception, string message)
        {
            FilteredLog(type, logger, LogLevel.Error, exception, message, null);
        }

        public static void Fatal(this ILogger logger, string name, Exception exception, string message)
        {
            FilteredLog(name, logger, LogLevel.Fatal, exception, message, null);
        }

        public static void Fatal(this ILogger logger, Type type, Exception exception, string message)
        {
            FilteredLog(type, logger, LogLevel.Fatal, exception, message, null);
        }

        public static void Debug(this ILogger logger, string name, string format, params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Debug, null, format, args);
        }

        public static void Debug(this ILogger logger, Type type, string format, params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Debug, null, format, args);
        }

        public static void Information(this ILogger logger, string name, string format, params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Information, null, format, args);
        }

        public static void Information(this ILogger logger, Type type, string format, params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Information, null, format, args);
        }

        public static void Warning(this ILogger logger, string name, string format, params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Warning, null, format, args);
        }

        public static void Warning(this ILogger logger, Type type, string format, params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Warning, null, format, args);
        }

        public static void Error(this ILogger logger, string name, string format, params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Error, null, format, args);
        }

        public static void Error(this ILogger logger, Type type, string format, params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Error, null, format, args);
        }

        public static void Fatal(this ILogger logger, string name, string format, params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Fatal, null, format, args);
        }

        public static void Fatal(this ILogger logger, Type type, string format, params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Fatal, null, format, args);
        }

        public static void Debug(this ILogger logger, string name, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Debug, exception, format, args);
        }

        public static void Debug(this ILogger logger, Type type, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Debug, exception, format, args);
        }

        public static void Information(this ILogger logger, string name, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Information, exception, format, args);
        }

        public static void Information(this ILogger logger, Type type, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Information, exception, format, args);
        }

        public static void Warning(this ILogger logger, string name, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Warning, exception, format, args);
        }

        public static void Warning(this ILogger logger, Type type, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Warning, exception, format, args);
        }

        public static void Error(this ILogger logger, string name, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Error, exception, format, args);
        }

        public static void Error(this ILogger logger, Type type, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Error, exception, format, args);
        }

        public static void Fatal(this ILogger logger, string name, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(name, logger, LogLevel.Fatal, exception, format, args);
        }

        public static void Fatal(this ILogger logger, Type type, Exception exception, string format,
            params object[] args)
        {
            FilteredLog(type, logger, LogLevel.Fatal, exception, format, args);
        }

        private static void FilteredLog(string name, ILogger logger, LogLevel level, Exception exception, string format,
            object[] objects)
        {
            if (logger.IsEnabled(name, level))
            {
                logger.Log(level, exception, format, objects);
            }
        }

        private static void FilteredLog(Type type, ILogger logger, LogLevel level, Exception exception, string format,
            object[] objects)
        {
            if (logger.IsEnabled(type, level))
            {
                logger.Log(level, exception, format, objects);
            }
        }
    }
}