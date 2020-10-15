using System;

namespace MyNLog.Models
{
    public class LogItem
    {
        public int Index { get; private set; }
        public DateTime Time { get; }
        public LogLevel Level { get; }
        public string Logger { get; }
        public string Message { get; }
        public string StackTrace { get; }
        public string Exception { get; }

        public LogItem(int index, DateTime time, LogLevel level, string logger, string message, string stackTrace, string exception)
        {
            Index = index;
            Time = time;
            Level = level;
            Logger = logger;
            Message = message;
            StackTrace = stackTrace;
            Exception = exception;
        }

        public LogItem(DateTime time, LogLevel level, string logger, string message, string stackTrace, string exception)
        {
            Time = time;
            Level = level;
            Logger = logger;
            Message = message;
            StackTrace = stackTrace;
            Exception = exception;
        }

        public void SetIndex(int index)
        {
            Index = index;
        }
    }
}
