using MyNLog.Models;
using Prism.Mvvm;
using System;

namespace MyNLog.ViewModels
{
    public class LogItemViewModel : BindableBase
    {
        private bool _expanded;
        public bool Expanded
        {
            get => _expanded;
            set => SetProperty(ref _expanded, value);
        }

        public DateTime Time => _logItem.Time;
        public LogLevel Level => _logItem.Level;
        public string Logger => _logItem.Logger;
        public string Message => _logItem.Message;
        public string StackTrace => _logItem.StackTrace;
        public string Exception => _logItem.Exception;

        public int Index => _logItem.Index;

        private LogItem _logItem;

        public LogItemViewModel(LogItem logItem)
        {
            _logItem = logItem;
        }

        public override string ToString()
        {
            return $"{Time:yyyy-MM-dd HH:mm:ss.ffff}\t{Level}\t{Message.Replace("\n", "\n    ")}\nLogger: {Logger}\nStackTrace: {StackTrace.Replace("\n", "\n    ")}" +
                (string.IsNullOrWhiteSpace(Exception) ? "" : $"\nException: {Exception.Replace("\n", "\n    ")}");
        }
    }
}
