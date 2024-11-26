using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Pan
{
    public struct LogItem
    {
        public PLogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public Type HolderType { get; set; }

        public Exception Exception  { get; set; }

        public Object Holder { get; set; }

        public IReadOnlyDictionary<string, object> ExtraData { get; set; }
    }
}
