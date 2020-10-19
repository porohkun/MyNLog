﻿using MyNLog.Models;
using MyNLog.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyNLog.ViewModels
{
    public class MainWindowViewModelDummy : MainWindowViewModel
    {
        public MainWindowViewModelDummy() : base()
        {
            LogItems.Add(new LogItem(0, DateTime.Now, LogLevel.Trace, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", ""));
            LogItems.Add(new LogItem(1, DateTime.Now, LogLevel.Debug, "MyNLog.Appp", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", ""));
            LogItems.Add(new LogItem(2, DateTime.Now, LogLevel.Info, "MyNLog.Apppp", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", ""));
            LogItems.Add(new LogItem(3, DateTime.Now, LogLevel.Warn, "MyNLog.Appppp", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32"));
            LogItems.Add(new LogItem(4, DateTime.Now, LogLevel.Error, "MyNLog.Apppppp", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32"));
            LogItems.Add(new LogItem(5, DateTime.Now, LogLevel.Fatal, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32"));
            LogItems.Add(new LogItem(6, DateTime.Now, LogLevel.Trace, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", ""));
        }
    }

    public class MainWindowViewModel : BindableBase
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(LogFileService).ToString());

        public ObservableCollection<LogItem> LogItems { get; } = new ObservableCollection<LogItem>();

        public DelegateCommand ClearLogCommand { get; }

        private LogFileService _logFileService;
        private int _minIndex = -1;

        protected MainWindowViewModel() { }

        public MainWindowViewModel(LogFileService logFileService)
        {
            ClearLogCommand = new DelegateCommand(ClearLog);

            _logFileService = logFileService;
            _logFileService.MaxIndexChanged += LogFileService_MaxIndexChanged;
            _logFileService.SourceDisconnected += _logFileService_SourceDisconnected;
        }

        private void LogFileService_MaxIndexChanged()
        {
            _minIndex = Math.Max(_minIndex, _logFileService.MinIndex);

            for (int i = _minIndex; i <= _logFileService.MaxIndex; i++)
                LogItems.Add(_logFileService.GetRecord(i));

            _minIndex = LogItems.Any() ? (LogItems.Last().Index + 1) : _logFileService.MinIndex;
        }

        private void _logFileService_SourceDisconnected()
        {
            _minIndex = -1;
            ClearLog();
        }

        private void ClearLog()
        {
            LogItems.Clear();
        }
    }
}
