using System;

namespace BrCms.Framework.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, Exception exception, string format, params object[] args);
        bool IsEnabled(string name, LogLevel level);
        bool IsEnabled(Type type, LogLevel level);
    }

    public enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }
}