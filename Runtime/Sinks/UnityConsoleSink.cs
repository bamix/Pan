using System;
using UnityEngine;

namespace Pan.Sinks
{
    public class UnityConsoleSink : ILogSink
    {
        private readonly Func<LogItem, string> logFunc;

        public UnityConsoleSink(Func<LogItem, string> logFunc)
        {
            this.logFunc = logFunc;
        }

        public UnityConsoleSink()
        {
            this.logFunc = log =>
            {
#if UNITY_EDITOR
                Func<string> logLevelFunc = log.LogLevel switch
                {
                    PLogLevel.Debug => () => "<b><color=white>[Debug]</color></b> ",
                    PLogLevel.Info => () => "<b><color=green>[Info]</color></b> ",
                    PLogLevel.Warn => () => "<b><color=orange>[Warn]</color></b> ",
                    PLogLevel.Error => () => "<b><color=red>[Error]</color></b> ",
                    _ => () => ""
                };

                var message = $"{logLevelFunc()}<color=orange><b>{log.HolderType.Name}</b></color> {log.Message}";

# else
                var message = $"[{log.LogLevel}]: [{log.HolderType.Name}] {log.Message}";
#endif

                return message;
            };
        }

        public void Write(LogItem logItem)
        {
            var message = this.logFunc(logItem);
            switch (logItem.LogLevel)
            {
                case PLogLevel.Error:
                    Debug.LogError(message, logItem.Holder);
                    break;
                case PLogLevel.Warn:
                    Debug.LogWarning(message, logItem.Holder);
                    break;
                default:
                    Debug.Log(message, logItem.Holder);
                    break;
            }

            if (logItem.Exception != null)
            {
                Debug.LogException(logItem.Exception, logItem.Holder);
            }
        }
    }
}