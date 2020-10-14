using System;

namespace MyNLog.Models
{
    public struct LogItem
    {
        public DateTime Time { get; }
        public LogLevel Level { get; }
        public string Logger { get; }
        public string Message { get; }
        public string StackTrace { get; }
        public string Exception { get; }

        public LogItem(DateTime time, LogLevel level, string logger, string message, string stackTrace, string exception)
        {
            Time = time;
            Level = level;
            Logger = logger;
            Message = message;
            StackTrace = stackTrace;
            Exception = exception;
        }
    }
}
