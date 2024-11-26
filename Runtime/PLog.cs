using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.PackageManager;
using Object = UnityEngine.Object;
#endif

namespace Pan
{
    public class PLog<T> : PLog
    {
        public PLog() : base(typeof(T))
        {
        }
    }

    public class PLog : ILog
    {
        public static PLogLevel MinLogLevel { get; set; } = PLogLevel.Debug;
        private static readonly List<ILogSink> LogSinks = new();

        public static void AddSink(ILogSink sink)
        {
            LogSinks.Add(sink);
        }

        private readonly Object holder;
        private readonly Type type;

        public PLog(Object holder)
        {
            this.holder = holder;
            this.type = holder.GetType();
        }

        public PLog(Type type)
        {
            this.type = type;
        }

        private Dictionary<string, object> extraData;

        public IDictionary<string, object> ExtraData => this.extraData ??= new Dictionary<string, object>();

        public void Debug(Action<ILog.FormatMessageHandler> message) => Log(PLogLevel.Debug, message);

        public void Info(Action<ILog.FormatMessageHandler> message) => Log(PLogLevel.Info, message);

        public void Warn(Action<ILog.FormatMessageHandler> message, Exception e = null) => Log(PLogLevel.Warn, message, e);

        public void Error(Action<ILog.FormatMessageHandler> message, Exception e = null) => Log(PLogLevel.Error, message, e);

        private void Log(PLogLevel logLevel, Action<ILog.FormatMessageHandler> message, Exception e = null)
        {
            if (logLevel < MinLogLevel)
            {
                return;
            }

            string formattedMessage = null;
            message((format, args) => formattedMessage = args?.Any() ?? false ? string.Format(format, args) : format);
            foreach (var sink in LogSinks)
            {
                try
                {
                    sink.Write(new LogItem
                    {
                        LogLevel = logLevel,
                        Message = formattedMessage,
                        Exception = e,
#if UNITY_EDITOR
                        Holder = this.holder,
#endif
                        HolderType = this.type,
                        ExtraData = this.extraData
                    });
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
