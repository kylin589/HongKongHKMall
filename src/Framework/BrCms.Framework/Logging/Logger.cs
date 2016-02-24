using System;
using log4net;

namespace BrCms.Framework.Logging
{
    public class Logger : ILogger
    {
        private ILog _log;

        public void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
            if (args == null)
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        this._log.Debug(format, exception);
                        break;
                    case LogLevel.Information:
                        this._log.Info(format, exception);
                        break;
                    case LogLevel.Warning:
                        this._log.Warn(format, exception);
                        break;
                    case LogLevel.Error:
                        this._log.Error(format, exception);
                        break;
                    case LogLevel.Fatal:
                        this._log.Fatal(format, exception);
                        break;
                }
            }
            else
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        this._log.DebugFormat(format, exception, args);
                        break;
                    case LogLevel.Information:
                        this._log.InfoFormat(format, exception, args);
                        break;
                    case LogLevel.Warning:
                        this._log.WarnFormat(format, exception, args);
                        break;
                    case LogLevel.Error:
                        this._log.ErrorFormat(format, exception, args);
                        break;
                    case LogLevel.Fatal:
                        this._log.FatalFormat(format, exception, args);
                        break;
                }
            }
        }

        public bool IsEnabled(string name, LogLevel level)
        {
            this._log = LogManager.GetLogger(name);
            return this.IsDebugEnabled(level);
        }

        public bool IsEnabled(Type type, LogLevel level)
        {
            this._log = LogManager.GetLogger(type);
            return this.IsDebugEnabled(level);
        }

        private bool IsDebugEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return this._log.IsDebugEnabled;
                case LogLevel.Information:
                    return this._log.IsInfoEnabled;
                case LogLevel.Warning:
                    return this._log.IsWarnEnabled;
                case LogLevel.Error:
                    return this._log.IsErrorEnabled;
                case LogLevel.Fatal:
                    return this._log.IsFatalEnabled;
            }
            return false;
        }
    }
}