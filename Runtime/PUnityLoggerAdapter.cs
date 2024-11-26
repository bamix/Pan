using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pan
{
    public class PUnityLoggerAdapter : ILogger
    {
        private readonly ILog log;

        public PUnityLoggerAdapter(ILog log)
        {
            this.log = log;
        }

        public bool IsLogTypeAllowed(LogType logType) => true;

        public void Log(LogType logType, object message)
        {
            switch (logType)
            {
                case LogType.Error:
                case LogType.Exception:
                    this.log.Error(m => m(message?.ToString()));
                    break;
                case LogType.Assert:
                    this.log.Info(m => m(message?.ToString()));
                    break;
                case LogType.Warning:
                    this.log.Warn(m => m(message?.ToString()));
                    break;
                default:
                    this.log.Debug(m => m(message?.ToString()));
                    break;
            }
        }

        public void Log(LogType logType, object message, Object context) => Log(logType, message);

        public void Log(LogType logType, string tag, object message) => Log(logType, message);

        public void Log(LogType logType, string tag, object message, Object context) => Log(logType, message);

        public void Log(object message) => Log(LogType.Log, message);

        public void Log(string tag, object message) => Log(LogType.Log, message);

        public void Log(string tag, object message, Object context) => Log(LogType.Log, message);

        public void LogWarning(string tag, object message) => Log(LogType.Warning, message);

        public void LogWarning(string tag, object message, Object context) => Log(LogType.Warning, message);

        public void LogError(string tag, object message) => Log(LogType.Error, message);

        public void LogError(string tag, object message, Object context) => Log(LogType.Error, message);

        public void LogFormat(LogType logType, string format, params object[] args) => Log(logType, string.Format(format, args));

        public void LogFormat(LogType logType, Object context, string format, params object[] args) => Log(logType, string.Format(format, args));

        public void LogException(Exception exception, Object context) => LogException(exception);

        public void LogException(Exception exception)
        {
            this.log.Error(m => m("Exception: {0}", exception.Message), exception);
        }

        public ILogHandler logHandler { get; set; }

        public bool logEnabled { get; set; }

        public LogType filterLogType { get; set; }
    }
}
