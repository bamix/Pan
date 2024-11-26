using System;
using System.Collections.Generic;

namespace Pan
{
    public interface ILog
    {
        IDictionary<string, object> ExtraData { get; }

        void Debug(Action<FormatMessageHandler> message);

        void Info(Action<FormatMessageHandler> message);

        void Warn(Action<FormatMessageHandler> message, Exception e = null);

        void Error(Action<FormatMessageHandler> message, Exception e = null);

        public delegate string FormatMessageHandler(string format, params object[] args);
    }
}
